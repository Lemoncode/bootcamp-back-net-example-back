using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Domain.Entities.Books;

namespace Lemoncode.LibraryExample.Application.Extensions.Mappers;

internal static class BookMapperExtensions
{

	public static Book ConvertToDomainEntity(this BookDto book, string imageUri)
	{
		return new Book(
			0, // Solo creación, por lo que no tenemos aún identificador. La modificación se hace con los métodos de la entidad.
			title: book.Title,
			new BookDescription(book.Description),
			new BookImage(imageUri, book.ImageAltText),
			authors: book.AuthorIds
			);
	}
}
