using Lemoncode.LibraryExample.Domain.Entities.Authors;
using Lemoncode.LibraryExample.Domain.Entities.Reviews;

namespace Lemoncode.LibraryExample.Domain.Entities;

public class Book
{

	public int Id { get; set; }

	public required string Title { get; set; }

	public required string Description { get; set; }

	public string? ImageAltText { get; set; }

	public DateTime Created { get; set; }

	public required ICollection<Author> Authors { get; set; }

	public required ICollection<Review> Reviews { get; set; }

	public required ICollection<BookDownload> Downloads { get; set; }
}
