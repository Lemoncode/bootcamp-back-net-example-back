using Google.Apis.Auth;
using Lemoncode.LibraryExample.AuthPlatform.Entities;

namespace Lemoncode.LibraryExample.AuthPlatform.Abstractions.IdentityProviders;

public interface IGoogleOauthService
{
	string GetOauthCodeUrl(string? returnUrl = null);

	public Task<GoogleCodeExchangeResponse> GetToken(string code);

	public Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string token);
}
