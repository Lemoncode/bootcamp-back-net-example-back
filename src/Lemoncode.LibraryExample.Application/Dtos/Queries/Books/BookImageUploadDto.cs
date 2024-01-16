namespace Lemoncode.LibraryExample.Application.Dtos.Queries.Books;

public class BookImageUploadDto
{

	public required string FileName { get; set; }

	public required string ContentType { get; set; }

	public required Stream BinaryData { get; set; }
}