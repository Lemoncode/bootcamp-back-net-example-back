using Lemoncode.LibraryExample.Domain.Entities.Authors;
using Lemoncode.LibraryExample.Domain.Entities.Exceptions;

namespace Lemoncode.LibraryExample.Domain.Entities.Books;

public class Book : Entity
{

	public int Id { get; private set; }

	public string Title { get; private set; }

	public BookDescription Description { get; private set; }

	public BookImage Image { get; private set; }


	public DateTime Created { get; set; }

	public DateTime Updated { get; private set; }

	public ICollection<int> Authors { get; private set; }

	public void UpdateTitle(string title)
	{
		this.Title = title;
		UpdateUpdatedDate();
		EnsureStateIsValid();
	}

	public void UpdateAuthors(ICollection<int> authors)
	{
		this.Authors = authors;
		UpdateUpdatedDate();
		EnsureStateIsValid();
	}

	public void UpdateDescription(string description)
	{
		this.Description = new BookDescription(description);
		UpdateUpdatedDate();
	}

	public void UpdateImage(string fileName, string altText)
	{
		this.Image = new BookImage(fileName, altText);
		UpdateUpdatedDate();
		EnsureStateIsValid();
	}


	public Book(int id, string title, BookDescription description, BookImage image, ICollection<int> authors)
	{
		this.Id = id;
		this.Title = title;
		this.Description = description;
		this.Image = image;
		this.Created = DateTime.UtcNow;
		UpdateUpdatedDate();
		this.Authors = authors;
		EnsureStateIsValid();
	}

	private void UpdateUpdatedDate()
	{
		this.Updated = DateTime.UtcNow;
	}

	protected override void EnsureStateIsValid()
	{
		if (string.IsNullOrWhiteSpace(Title))
		{
			AddValidationError("The title is mandatory.");
		}

		if (!Authors.Any())
		{
			AddValidationError("The book should have at least one author.");
		}

		Validate();
	}
}
