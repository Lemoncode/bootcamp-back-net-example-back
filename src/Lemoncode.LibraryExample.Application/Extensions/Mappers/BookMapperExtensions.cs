using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Domain.Entities.Books;

namespace Lemoncode.LibraryExample.Application.Extensions.Mappers;

internal static class BookMapperExtensions
{

	public static Book ConvertToDomainEntity(this BookDto book, string imageUri)
	{
		return new Book(
			id: book.Id,
			title: book.Title,
			new BookDescription(book.Description),
			new BookImage(imageUri, book.ImageAltText),
			authors: book.AuthorIds
			);
	}
}
