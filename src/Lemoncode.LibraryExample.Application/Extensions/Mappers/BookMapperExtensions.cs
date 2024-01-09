using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;
using Lemoncode.LibraryExample.Domain.Entities.Authors;
using Lemoncode.LibraryExample.Domain.Entities.Books;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Extensions.Mappers;

internal static class BookMapperExtensions
{

	public static Book ConvertToDomainEntity(this AddOrEditBookDto book, string imageFileName, DateTime created, DateTime updated)
	{
		return new Book(
			id: book.Id,
			title: book.Title,
			new BookDescription(book.Description),
			new BookImage(imageFileName, book.ImageAltText),
			created: created,
			updated: updated,
			authors: book.AuthorIds
			);
	}

}
