using Lemoncode.LibraryExample.Domain.Abstractions.Entities;

namespace Lemoncode.LibraryExample.DataAccess.Entities;

public class Book : IIdentifiable
{

	public int Id { get; set; }

	public required string Title { get; set; }

	public required string Description { get; set; }

	public required string ImageAltText { get; set; }

	public DateTime Created { get; set; }

	public double AVerage { get; set; }

	public required ICollection<Author> Authors { get; set; }

	public required ICollection<Review> Reviews { get; set; }
	
	public required ICollection<BookDownload> Downloads { get; set; }
}
