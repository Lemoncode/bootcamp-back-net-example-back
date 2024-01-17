using Lemoncode.LibraryExample.Api.Config;
using Lemoncode.LibraryExample.AuthPlatform.Abstractions;
using Lemoncode.LibraryExample.AuthPlatform.Abstractions.IdentityProviders;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json.Linq;

using System.ComponentModel.DataAnnotations;

namespace Lemoncode.LibraryExample.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExternalLoginController(IJWTService jwtService, IGoogleOauthService googleOauthService, IMicrosoftOauthService microsoftOauthService, IOptionsSnapshot<FrontendConfig> frontendConfig) : Controller
{

	private readonly IJWTService _jwtService = jwtService;

	private readonly IOptionsSnapshot<FrontendConfig> _frontendConfig = frontendConfig;
	
	private readonly IGoogleOauthService _googleOauthService = googleOauthService;
	
	private readonly IMicrosoftOauthService _MicrosoftOauthService = microsoftOauthService;

	[HttpGet("initiateGoogleSignin")]
	public IActionResult InitiateGoogleSignin([FromQuery]string? returnUrl)
	{
		var url = _googleOauthService.GetOauthCodeUrl(returnUrl);
		return Redirect(url);
	}

	[HttpGet("googleSignin")]
	public async Task<IActionResult> GoogleSignin([FromQuery, Required] string code, [FromQuery]string? state)
	{
		var tokenResponse = await _googleOauthService.GetToken(code);
		var payload = await _googleOauthService.GetUserInfo(tokenResponse.IdToken);
		var token = _jwtService.GenerateJwtToken(payload.FamilyName, payload.GivenName, payload.Email);

		this.Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Secure = true, Expires = DateTime.Now.AddMinutes(30) });
		return Redirect(_frontendConfig.Value.FrontendBaseUrl + (!string.IsNullOrWhiteSpace(state) ? state: string.Empty));
	}

	[HttpGet("initiateMicrosoftSignin")]
	public IActionResult InitiateMicrosoftSignin([FromQuery] string? returnUrl)
	{
		var url = _microsoftOauthService.GetOauthCodeUrl(returnUrl);
		return Redirect(url);
	}

	[HttpGet("googleSignin")]
	public async Task<IActionResult> GoogleSignin([FromQuery, Required] string code, [FromQuery] string? state)
	{
		var tokenResponse = await _googleOauthService.GetToken(code);
		var payload = await _googleOauthService.GetUserInfo(tokenResponse.IdToken);
		var token = _jwtService.GenerateJwtToken(payload.FamilyName, payload.GivenName, payload.Email);

		this.Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Secure = true, Expires = DateTime.Now.AddMinutes(30) });
		return Redirect(_frontendConfig.Value.FrontendBaseUrl + (!string.IsNullOrWhiteSpace(state) ? state : string.Empty));
	}


	[HttpGet("logout")]
	public IActionResult Logout([FromQuery]string? returnUrl)
	{
		this.Response.Cookies.Delete("AuthToken");
		return Redirect(_frontendConfig.Value.FrontendBaseUrl + (returnUrl ?? string.Empty));
	}
}
