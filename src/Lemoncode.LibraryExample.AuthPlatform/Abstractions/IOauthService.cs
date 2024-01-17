using Google.Apis.Auth;

using Lemoncode.LibraryExample.AuthPlatform.Entities;

namespace Lemoncode.LibraryExample.AuthPlatform.Abstractions;

public interface IOauthService
{
	string GetOauthCodeUrl(string? returnUrl = null);

	public Task<OauthCodeExchangeResponse> GetToken(string code);
}
