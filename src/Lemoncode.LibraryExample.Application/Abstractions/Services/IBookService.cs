using Lemoncode.LibraryExample.Application.Dtos;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services;

public interface IBookService
{
	Task<int> AddBook(AddOrEditBookDto book);
	
	Task EditBook(int bookId, AddOrEditBookDto book);

	Task DeleteBook(int bookId);
	
	Task<IEnumerable<BookDto>> GetMostDownloadedBooksAsync();

	Task<IEnumerable<BookDto>> GetNoveltiesAsync(int limit);
	
	Task<IEnumerable<BookDto>> GetTopRatedBooksAsync();

	Task<IEnumerable<BookDto>> SearchByTitleAsync(string text);
}