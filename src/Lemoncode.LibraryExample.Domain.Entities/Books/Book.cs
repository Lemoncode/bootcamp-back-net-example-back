using Lemoncode.LibraryExample.Domain.Entities.Authors;
using Lemoncode.LibraryExample.Domain.Entities.Exceptions;

namespace Lemoncode.LibraryExample.Domain.Entities.Books;

public class Book : Entity
{

	private ICollection<Review> _reviews = new List<Review>();

	public int Id { get; private set; }

	public string Title { get; private set; }

	public BookDescription Description { get; private set; }

	public BookImage Image { get; private set; }


	public DateTime Created { get; set; }

	public DateTime Updated { get; private set; }

	public ICollection<int> Authors { get; private set; }

	public Book(int id, string title, BookDescription description, BookImage image, DateTime created, DateTime updated, ICollection<int> authors)
	{
		if (string.IsNullOrWhiteSpace(title))
		{
			AddValidationError("The title is mandatory.");
		}

		if (!authors.Any())
		{
			AddValidationError("The book should have at least one author.");
		}

		Validate();		

		this.Id = id;
		this.Title = title;
		this.Description = description;
		this.Image = image;
		this.Created = created;
		this.Updated = updated;
		this.Authors = authors;
	}

	public void UpdateDescription(string description)
	{
		this.Description = new BookDescription(description);
	}

	public void AddReview(int reviewId, string reviewer, ReviewText text, DateTime creationDate, ushort stars)
	{
		var review = new Review(reviewId, this.Id, reviewer, text, creationDate, stars);
		this._reviews.Add(review);
	}
}
