namespace Lemoncode.LibraryExample.Domain.Entities;

public class Review
{

	public int Id { get; set; }

	public required Book Book { get; set; }

	public required string Reviewer { get; set; }

	public required string ReviewText { get; set; }

	public ushort Stars { get; set; }

}
