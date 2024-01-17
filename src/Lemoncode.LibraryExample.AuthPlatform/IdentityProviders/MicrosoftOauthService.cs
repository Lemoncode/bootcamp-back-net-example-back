using Lemoncode.LibraryExample.AuthPlatform.Abstractions.IdentityProviders;
using Lemoncode.LibraryExample.AuthPlatform.Config;
using Lemoncode.LibraryExample.AuthPlatform.Config.Microsoft;
using Lemoncode.LibraryExample.AuthPlatform.Entities;
using Lemoncode.LibraryExample.AuthPlatform.Entities.Microsoft;
using Lemoncode.LibraryExample.AuthPlatform.Exceptions;

using Microsoft.Extensions.Options;

using System.Net;
using System.Text.Json;

namespace Lemoncode.LibraryExample.AuthPlatform.IdentityProviders;

public class MicrosoftOauthService : OauthService, IMicrosoftOauthService
{

	public MicrosoftOauthService(IOptionsSnapshot<MicrosoftOauthConfig> oauthConfig, IHttpClientFactory httpClientFactory) : base(oauthConfig, httpClientFactory)
	{
	}

	public MicrosoftPayload GetUserInfo(string token)
	{
		try
		{
			var payload = GetPayload<MicrosoftPayload>(token);
			return payload;
		}
		catch (Exception ex)
		{
			throw new InvalidTokenException("Error when validating and retrieving information from Microsoft's token.", ex);
		}
	}
}