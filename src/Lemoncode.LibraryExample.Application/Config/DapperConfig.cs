using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Config
{
	public record class DapperConfig
	{
		public static readonly string ConfigurationSection = "ConnectionStrings";

		public required string DefaultConnectionString { get; set; }

	}
}
