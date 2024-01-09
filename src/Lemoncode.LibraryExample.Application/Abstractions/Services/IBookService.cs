using FluentValidation.Results;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services;

public interface IBookService
{
	Task<(ValidationResult ValidationResult, string? ImageId)> UploadBookImage(IFormFile file);

	Task<(ValidationResult ValidationResult, BookDto? book)> AddBook(AddOrEditBookDto book);

	Task<ValidationResult>  EditBook(AddOrEditBookDto book);

	Task DeleteBook(int bookId);
}