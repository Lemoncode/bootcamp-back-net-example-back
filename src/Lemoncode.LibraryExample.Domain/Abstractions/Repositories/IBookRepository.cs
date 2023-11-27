using Lemoncode.LibraryExample.Domain.Abstractions.Entities;
using Lemoncode.LibraryExample.Domain.Entities;
using Lemoncode.LibraryExample.Domain.Entities.Books;

namespace Lemoncode.LibraryExample.Domain.Abstractions.Repositories;

public interface IBookRepository
{

	Task<Book> GetBook(int bookId);

	Task<IEnumerable<Book>> GetNovelties(int limit);

	Task<IEnumerable<Book>> Search(string text);

	Task<IEnumerable<Book>> GetTopRatedBooks();

	Task<IEnumerable<Book>> GetMostDownloadedBooksAsync();

	Task<IIdentifiable> AddBook(AddOrEditBook book);

	Task EditBook(int bookId, AddOrEditBook book);

	Task DeleteBook(int bookId);
	Task<bool> BookExists(int bookId);
}
