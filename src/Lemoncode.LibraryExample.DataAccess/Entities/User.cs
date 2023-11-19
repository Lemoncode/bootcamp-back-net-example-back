using Lemoncode.LibraryExample.Domain.Abstractions.Entities;

namespace Lemoncode.LibraryExample.DataAccess.Entities;

public class User : IIdentifiable
{

	public int Id { get; set; }

	public required string Name { get; set; }

	public required string LastName { get; set; }

	public required string Email { get; set; }
	
	public ICollection<BookDownload>? BookDownloads { get; set; }
}
