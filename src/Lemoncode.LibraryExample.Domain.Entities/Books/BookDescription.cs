using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Domain.Entities.Books;

public record class BookDescription
{

	public string Description { get; private set; }

	public BookDescription(string description)
	{
		Description = description ?? throw new ArgumentNullException(nameof(BookDescription));
	}

	public static BookDescription CreateFromString(string str)
	{
		str = "<p>" + str.Replace("\r", "").Replace("\n", "</p><p>") + "</p>";
		return new BookDescription(str);
	}
}