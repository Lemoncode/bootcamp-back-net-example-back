using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.AuthPlatform.Config.Google;

public record class GoogleOauthConfig : OauthConfig
{
	public static readonly string ConfigSection = "GoogleSso";

}
