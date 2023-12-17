using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Dtos.Books;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services;

public interface IBookService
{
	FileStreamResult GetBookImage(int bookId);

	Task<BookDto> GetBook(int bookId);

	Task<IEnumerable<BookDto>> GetMostDownloadedBooksAsync();

	Task<IEnumerable<BookDto>> GetNoveltiesAsync(int limit);

	Task<IEnumerable<BookDto>> GetTopRatedBooksAsync();

	Task<(ValidationResult ValidationResult, string? ImageId)> UploadBookImage(IFormFile file);

	Task<(ValidationResult ValidationResult, BookDto? book)> AddBook(AddOrEditBookDto book);

	Task<ValidationResult>  EditBook(AddOrEditBookDto book);

	Task DeleteBook(int bookId);
}