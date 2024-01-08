using Lemoncode.LibraryExample.Domain.Entities.Authors;
using Lemoncode.LibraryExample.Domain.Entities.Reviews;

namespace Lemoncode.LibraryExample.Domain.Entities;

public class Book
{
	public Book(int id, string title, string description, DateTime created, DateTime updated, ICollection<Author> authors, ICollection<Review> reviews)
	{
		Id = id;
		Title = title ?? throw new ArgumentNullException(nameof(title));
		Description = description ?? throw new ArgumentNullException(nameof(description));
		Created = created;
		Updated = updated;
		Authors = authors ?? throw new ArgumentNullException(nameof(authors));
		Reviews = reviews ?? throw new ArgumentNullException(nameof(reviews));
	}

	public int Id { get; private set; }

	public string Title { get; private  set; }

	public string Description { get; private set; }

	public DateTime Created { get; set; }

	public DateTime Updated { get; private set; }

	public ICollection<Author> Authors { get; private set; }

	public ICollection<Review> Reviews { get; private set; } = new List<Review>();

}
