using Lemoncode.LibraryExample.AuthPlatform.Abstractions;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace Lemoncode.LibraryExample.Api.Middlewares;

public class SlidingExpirationMiddleware
{
	private readonly RequestDelegate _next;
	
	private readonly IJWTService _jwtService;

	public SlidingExpirationMiddleware(RequestDelegate next, IJWTService jwtService)
	{
		_next = next;
		_jwtService = jwtService;
	}

	public async Task Invoke(HttpContext context)
	{
		var cookie = context.Request.Cookies["AuthToken"];
		if (cookie is not null)
		{
			var cookieOptions = new CookieOptions();
			if (ShouldRenew(cookie, out var newExpiration, out var newToken))
			{
				cookieOptions.Expires = newExpiration;
				context.Response.Cookies.Append("AuthToken", newToken, cookieOptions);
			}
		}

		await _next(context);
	}

	private bool ShouldRenew(string cookieValue, out DateTimeOffset newExpiration, out string newToken)
	{
		var jsonToken = _jwtService.ParseToken(cookieValue);
		var expiration = jsonToken.ValidTo;
		if (expiration - DateTime.UtcNow < TimeSpan.FromMinutes(5))
		{
			newExpiration = DateTimeOffset.UtcNow.AddMinutes(30);
			var familyName = jsonToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.FamilyName)?.Value;
			var givenName = jsonToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.GivenName)?.Value;
			var email = jsonToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
			newToken = _jwtService.GenerateJwtToken(familyName, givenName, email);
			return true;
		}

		newExpiration = default;
		newToken = default;
		return false;
	}
}
