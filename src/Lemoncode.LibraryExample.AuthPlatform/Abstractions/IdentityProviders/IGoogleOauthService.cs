using Google.Apis.Auth;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.AuthPlatform.Abstractions.IdentityProviders
{
	public interface IGoogleOauthService  : IOauthService
	{
		Task<GoogleJsonWebSignature.Payload> GetUserInfo(string token);
	}
}
