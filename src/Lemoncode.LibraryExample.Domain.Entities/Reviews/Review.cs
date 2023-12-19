namespace Lemoncode.LibraryExample.Domain.Entities.Reviews;

public class Review
{

	public int Id { get; set; }

	public int BookId { get; set; }

	public required string Reviewer { get; set; }

	public required string ReviewText { get; set; }

	public DateTime CreationDate { get; set; }

	public ushort Stars { get; set; }

}
