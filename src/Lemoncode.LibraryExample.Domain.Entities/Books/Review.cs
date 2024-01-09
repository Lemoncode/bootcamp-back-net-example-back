using Lemoncode.LibraryExample.Domain.Entities.Exceptions;

using System.Collections.Generic;

namespace Lemoncode.LibraryExample.Domain.Entities.Books;

public class Review : Entity
{

	public int Id { get; private set; }

	public int BookId { get; private set; }

	public string Reviewer { get; private set; }

	public ReviewText Text { get; private set; }

	public DateTime CreationDate { get; private set; }

	public ushort Stars { get; private set; }

	public Review(int id, int bookId, string reviewer, ReviewText text, ushort stars)
	{
		this.Id = id;
		this.BookId = bookId;
		this.Reviewer = reviewer;
		this.Text = text;
		this.CreationDate = DateTime.UtcNow;
		this.Stars = stars;

		EnsureStateIsValid();
	}

	public void UpdateReviewer(string reviewer)
	{
		this.Reviewer = reviewer;
		EnsureStateIsValid();
	}

	public void UpdateText(string text)
	{
		this.Text = new ReviewText(text);
		EnsureStateIsValid();
	}
	
	public void UpdateStars(ushort stars)
	{
		this.Stars = stars;
		EnsureStateIsValid();
	}
	
	protected override void EnsureStateIsValid()
	{
		if (string.IsNullOrWhiteSpace(Reviewer))
		{
			AddValidationError("The name of the reviewer is mandatory.");
		}
		if (Stars < 1 || Stars > 5)
		{
			AddValidationError("The number of stars should be in the range 1 - 5.");
		}

		Validate();
	}
}
