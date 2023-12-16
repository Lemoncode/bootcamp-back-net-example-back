using Lemoncode.LibraryExample.Application.Dtos.Authors;

namespace Lemoncode.LibraryExample.Application.Dtos.Books;

public class BookDto
{
	public int Id { get; set; }

	public required string Title { get; set; }

	public required string Description { get; set; }

	public string? ImageUrl { get; set; }

	public string? ImageAltText { get; set; }

	public DateTime Created { get; set; }

	public required List<AuthorDto> Authors { get; set; }

	public required List<ReviewDto> Reviews { get; set; }
}