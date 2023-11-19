using Lemoncode.LibraryExample.Domain.Abstractions.Entities;

namespace Lemoncode.LibraryExample.DataAccess.Entities;

public class Review : IIdentifiable
{

	public int Id { get; set; }

	public required Book Book { get; set; }

	public required string Reviewer { get; set; }

	public required string ReviewText { get; set; }

	public ushort Stars { get; set; }

}
