using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Domain.Entities.Books;

public record class ReviewText : ValueObject
{

	public string Text { get; private set; }

	public ReviewText(string text)
	{
		if (text is null || text.Length < 10 || text.Length > 4000)
		{
			AddValidationError("The review text must contain between 10 and 4000 charracters.");
		}

		Validate();

		this.Text = text;
	}
}
