using Lemoncode.LibraryExample.Domain.Entities;

namespace Lemoncode.LibraryExample.Domain.Abstractions.Repositories;

public interface IBookRepository
{

	Task<IEnumerable<Book>> GetNoveltiesAsync(int limit);

	Task<IEnumerable<Book>> SearchByTitleAsync(string text);

	Task<IEnumerable<Book>> GetTopRatedBooksAsync();

	Task<IEnumerable<Book>> GetMostDownloadedBooksAsync();

	Task AddBook(Book book);

	void DeleteBook(Book book);
}
