using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;

namespace Lemoncode.LibraryExample.Application.Abstractions.Queries
{
	public interface IBookQueriesService
	{

		Task<BookImageUploadDto> GetBookImage(int bookId);

		Task<BookDto> GetBook(int bookId);
		Task<IList<BookDto>> GetNoveltiesAsync(int limit);
	}
}
