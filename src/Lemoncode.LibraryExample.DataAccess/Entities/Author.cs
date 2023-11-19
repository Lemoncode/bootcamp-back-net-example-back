using Lemoncode.LibraryExample.Domain.Abstractions.Entities;

namespace Lemoncode.LibraryExample.DataAccess.Entities;

public class Author : IIdentifiable
{
	public int Id { get; set; }

	public required string FirstName { get; set; }

	public required string LastName { get; set; }

	public required ICollection<Book> Books { get; set; }
}
