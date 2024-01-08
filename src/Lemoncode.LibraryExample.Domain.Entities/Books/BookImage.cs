using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Domain.Entities.Books
{
	public record class BookImage(string fileName, string altText)
	{

		public required string FileName { get; set; } = fileName;

		public required string AltText { get; set; } = altText;

	}
}
