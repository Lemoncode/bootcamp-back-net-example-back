namespace Lemoncode.LibraryExample.DataAccess.Entities;

public class BookDownload
{

	public int Id { get; set; }

	public required Book Book { get; set; }

	public required User User { get; set; }

	public required string IPAddress { get; set; }

	public required DateTime Date { get; set; }

}
