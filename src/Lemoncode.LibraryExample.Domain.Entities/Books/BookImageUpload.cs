using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Domain.Entities.Books;

public class BookImageUpload
{

	public required string FileName { get; set; }

	public required string ContentType { get; set; }

	public long Length { get; set; }

	public required Stream BinaryData { get; set; }
}
