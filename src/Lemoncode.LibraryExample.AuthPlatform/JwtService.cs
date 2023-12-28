using Lemoncode.LibraryExample.AuthPlatform.Abstractions;
using Lemoncode.LibraryExample.AuthPlatform.Config;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lemoncode.LibraryExample.Api.Services;

public class JwtService(IOptionsSnapshot<JwtConfig> jwtConfig) : IJWTService
{

	private readonly IOptionsSnapshot<JwtConfig> _jwtConfig = jwtConfig;
	
	public string GenerateJwtToken(string familyName, string givenName, string emailAddress)
	{
		var claims = new[]
		{
	new Claim(JwtRegisteredClaimNames.Sub, emailAddress),
	new Claim(JwtRegisteredClaimNames.FamilyName, familyName),
	new Claim(JwtRegisteredClaimNames.GivenName, givenName)
    };

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Value.SigningKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _jwtConfig.Value.Issuer,
			audience: _jwtConfig.Value.Audience,
			claims: claims,
			expires: DateTime.Now.AddMinutes(30),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
