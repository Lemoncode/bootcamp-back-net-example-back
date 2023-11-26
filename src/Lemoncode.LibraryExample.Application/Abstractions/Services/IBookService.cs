using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Dtos.Books;
using Microsoft.AspNetCore.Http;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services;

public interface IBookService
{
	Task<int> AddBook(AddOrEditBookDto book);
	
	Task EditBook(int bookId, AddOrEditBookDto book);

	Task DeleteBook(int bookId);
	
	Task<IEnumerable<BookDto>> GetMostDownloadedBooksAsync();

	Task<IEnumerable<BookDto>> GetNoveltiesAsync(int limit);
	
	Task<IEnumerable<BookDto>> GetTopRatedBooksAsync();

	Task<(ValidationResult ValidationResult, string ImageId)> UploadBookImage(IFormFile file);
}