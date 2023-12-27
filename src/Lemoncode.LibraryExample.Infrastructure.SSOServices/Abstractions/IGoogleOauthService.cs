using Google.Apis.Auth;

using Lemoncode.LibraryExample.SsoServices.Entities;

namespace Lemoncode.LibraryExample.SsoServices.Abstractions;

public interface IGoogleOauthService 
{
	string GetOauthCodeUrl();
	
	public Task<GoogleCodeExchangeResponse> GetToken(string code);

	public Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string token);
}
