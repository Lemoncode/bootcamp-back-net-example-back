using Lemoncode.LibraryExample.Domain.Entities.Exceptions;

namespace Lemoncode.LibraryExample.Domain.Entities.Books;

public class Review : Entity
{

	public int Id { get; private set; }

	public int BookId { get; private set; }

	public string Reviewer { get; private set; }

	public ReviewText Text { get; private set; }

	public DateTime CreationDate { get; private set; }

	public ushort Stars { get; private set; }

	public Review(int id, int bookId, string reviewer, ReviewText text, DateTime creationDate, ushort stars)
	{
		if (string.IsNullOrWhiteSpace(reviewer))
		{
			AddValidationError("The name of the reviewer is mandatory.");
		}
		if (stars < 1 || stars > 5)
		{
			AddValidationError("The number of stars should be in the range 1 - 5.");
		}

		Validate();

		Id = id;
		BookId = bookId;
		Reviewer = reviewer;
		Text = text;
		CreationDate = creationDate;
		Stars = stars;
	}
}
