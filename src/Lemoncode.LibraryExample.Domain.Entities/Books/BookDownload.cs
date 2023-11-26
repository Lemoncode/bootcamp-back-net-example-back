namespace Lemoncode.LibraryExample.Domain.Entities;

public class BookDownload
{

	public int Id { get; set; }

	public int BookId { get; set; }

	public int UserId { get; set; }

	public required string IPAddress { get; set; }

	public required DateTime Date { get; set; }

}
