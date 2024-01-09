using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Domain.Entities.Books;

public record class ReviewText
{

	public string Text { get; private set; }

	public ReviewText(string text)
	{
		Text = text ?? throw new ArgumentNullException(nameof(text));
	}
}
