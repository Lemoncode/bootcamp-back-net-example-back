using Lemoncode.LibraryExample.AuthPlatform.Entities.Microsoft;

namespace Lemoncode.LibraryExample.AuthPlatform.Abstractions.IdentityProviders
{
	public interface IMicrosoftOauthService : IOauthService
	{
		Task<GraphUserResponse> GetUserInfo(string token);
	}
}
