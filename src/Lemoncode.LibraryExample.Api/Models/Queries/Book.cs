using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;

namespace Lemoncode.LibraryExample.Api.Models.Queries;

public class Book : BookDto
{

	public required string ImageUrl { get; set; }
}
