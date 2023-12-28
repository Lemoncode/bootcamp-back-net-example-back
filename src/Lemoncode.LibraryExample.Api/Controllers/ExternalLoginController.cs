using Lemoncode.LibraryExample.Api.Config;
using Lemoncode.LibraryExample.AuthPlatform.Abstractions;
using Lemoncode.LibraryExample.AuthPlatform.Abstractions.IdentityProviders;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.ComponentModel.DataAnnotations;

namespace Lemoncode.LibraryExample.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExternalLoginController(IJWTService jwtService, IGoogleOauthService googleOauthService, IOptionsSnapshot<FrontendConfig> frontendConfig) : Controller
{

	private readonly IJWTService _jwtService = jwtService;

	private readonly IOptionsSnapshot<FrontendConfig> _frontendConfig = frontendConfig;
	
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
		return Redirect(_frontendConfig.Value.FrontendCompleteSigninUrl);
	}
}
