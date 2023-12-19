namespace Lemoncode.LibraryExample.Domain.Entities.Reviews;

public class AddOrEditReview
{

	public int Id { get; set; }

	public int BookId { get; set; }

	public required string Reviewer { get; set; }

	public required string ReviewText { get; set; }

	public ushort Stars { get; set; }

}
