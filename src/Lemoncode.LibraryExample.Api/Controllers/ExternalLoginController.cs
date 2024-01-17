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
		var url = _MicrosoftOauthService.GetOauthCodeUrl(returnUrl);
		return Redirect(url);
	}

	[HttpGet("microsoftSignin")]
	public async Task<IActionResult> MicrosoftSignin([FromQuery, Required] string code, [FromQuery] string? state)
	{
		var tokenResponse = await _MicrosoftOauthService.GetToken(code);
		var userInfo = await _MicrosoftOauthService.GetUserInfo(tokenResponse.AccessToken);
		var token = _jwtService.GenerateJwtToken(userInfo.Surname, userInfo.GivenName, userInfo.Mail);

		this.Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Secure = true, Expires = DateTime.Now.AddMinutes(30) });
		return Redirect(_frontendConfig.Value.FrontendBaseUrl + (!string.IsNullOrWhiteSpace(state) ? state : string.Empty));
	}

	[HttpPost("logout")]
	public IActionResult Logout()
	{
		this.Response.Cookies.Delete("AuthToken");
		return Ok();
	}
}
