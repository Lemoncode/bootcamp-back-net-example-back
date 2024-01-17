using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.AuthPlatform.Config.Microsoft;

public record class MicrosoftOauthConfig : OauthConfig
{
	public static readonly string ConfigSection = "MicrosoftSso";
}
