using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Dtos.Books;

public class AddOrEditBookDto
{

	public required string Title { get; set; }

	public required int[] AuthorIds { get; set; }

	public required string Description { get; set; }

	public string? ImageAltText { get; set; }

	public required DateTime Created { get; set; }


}
