using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.SsoServices.Config;

public record class FrontendConfig
{
	public static readonly string ConfigSection = "Frontend";

	public required string FrontendSigninUrl { get; set; }
}
