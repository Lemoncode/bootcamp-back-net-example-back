using System.Text.Json.Serialization;

namespace Lemoncode.LibraryExample.Application.Dtos.Commands.Books;

public class BookDto
{
	public enum OperationType
	{
		Add,
		Edit
	}

	[JsonIgnore]
	public OperationType Operation { get; set; }

	public required string Title { get; set; }

	public required int[] AuthorIds { get; set; }

	public required string Description { get; set; }

	public string? TempImageFileName { get; set; }

	public required string ImageAltText { get; set; }

}
