using Lemoncode.LibraryExample.Domain.Entities;
using Lemoncode.LibraryExample.Domain.Entities.Books;

namespace Lemoncode.LibraryExample.Domain.Abstractions.Services;

public interface IBookService
{
	(Stream Stream, string FileName) GetBookImage(int bookId);

	Task<Book> GetBook(int bookId);

	Task<IEnumerable<Book>> GetNovelties(int limit);

	Task<IEnumerable<Book>> Search(string text);

	Task<IEnumerable<Book>> GetTopRatedBooks();

	Task<IEnumerable<Book>> GetMostDownloadedBooksAsync();

	Task<Book> AddBook(AddOrEditBook book);

	Task<string> UploadBookImage(BookImageUpload bookImageUpload);

	Task EditBook(int bookId, AddOrEditBook book);

	Task DeleteBook(int bookId);
}