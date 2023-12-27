using Lemoncode.LibraryExample.Api.Abstractions;
using Lemoncode.LibraryExample.Api.Config;
using Lemoncode.LibraryExample.SsoServices.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Lemoncode.LibraryExample.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ExternalLoginController(IJWTService jwtService, IGoogleOauthService googleOauthService, IFrontendService frontendService) : Controller
	{

		private readonly IJWTService _jwtService = jwtService;

		private readonly IFrontendService _frontendService = frontendService;
		
		private readonly IGoogleOauthService _googleOauthService = googleOauthService;

		[HttpGet("initiateGoogleSignin")]
		public IActionResult InitiateGoogleSignin()
		{
			var url = _googleOauthService.GetOauthCodeUrl();
			return Redirect(url);
		}

		[HttpGet("googleSignin")]
		public async Task<IActionResult> GoogleSignin([FromQuery, Required] string code)
		{
			var tokenResponse = await _googleOauthService.GetToken(code);
			var payload = await _googleOauthService.ValidateGoogleToken(tokenResponse.IdToken);
			var token = _jwtService.GenerateJwtToken(payload.FamilyName, payload.GivenName, payload.Email);

			this.Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Secure = true, Expires = DateTime.Now.AddMinutes(30) });
			await _frontendService.SignIn(JsonSerializer.Serialize(new { GivenName = payload.GivenName, FamilyName = payload.FamilyName }));
			return Ok(new { GivenName = payload.GivenName, access_token = token });
		}
	}
}
