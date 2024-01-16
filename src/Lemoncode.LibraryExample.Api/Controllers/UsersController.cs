using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace Lemoncode.LibraryExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
{
	[Authorize]
	[HttpGet("me")]
	public IActionResult Me()
	{
		var claims = HttpContext.User?.Claims;
		if (claims is null)
		{
			return new StatusCodeResult((int)HttpStatusCode.Forbidden);
		}

		var result = new { GivenName = claims.Single(c => c.Type == ClaimTypes.GivenName).Value, FamilyName = claims.Single(c => c.Type == ClaimTypes.Surname).Value, EmailAddress = claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value };
		return Ok(result);
	}
}
