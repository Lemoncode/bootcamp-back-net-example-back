using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.AuthPlatform.Entities.Microsoft
{
	public record class IdTokenPayload
	{

		public required string FamilyName { get; set; }

		public required string GivenName { get; set; }

		public required string Email { get; set; }
	}
}
