using Google.Apis.Auth;

using Lemoncode.LibraryExample.AuthPlatform.Abstractions;
using Lemoncode.LibraryExample.AuthPlatform.Abstractions.IdentityProviders;
using Lemoncode.LibraryExample.AuthPlatform.Config;
using Lemoncode.LibraryExample.AuthPlatform.Config.Google;
using Lemoncode.LibraryExample.AuthPlatform.Exceptions;

using Microsoft.Extensions.Options;

namespace Lemoncode.LibraryExample.AuthPlatform.IdentityProviders;

public class GoogleOauthService : OauthService, IGoogleOauthService
{

	public GoogleOauthService(IOptionsSnapshot<GoogleOauthConfig> oauthConfig, IHttpClientFactory httpClientFactory) : base(oauthConfig, httpClientFactory)
	{
	}

	public async Task<GoogleJsonWebSignature.Payload> GetUserInfo(string token)
	{
		try
		{
			var payload = await GoogleJsonWebSignature.ValidateAsync(token);
			return payload;
		}
		catch (Exception ex)
		{
			throw new InvalidTokenException("Error when validating and retrieving information from Google's token.", ex);
		}
	}
}