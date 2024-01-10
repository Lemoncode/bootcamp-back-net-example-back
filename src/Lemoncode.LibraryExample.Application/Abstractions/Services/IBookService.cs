using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services;

public interface IBookService
{
	Task<(ValidationResult ValidationResult, Uri? ImageUri)> UploadBookImage(BookImageUploadDto file);

	Task<(ValidationResult ValidationResult, int? book)> AddBook(Dtos.Commands.Books.BookDto book);

	Task<ValidationResult> EditBook(Dtos.Commands.Books.BookDto book);

	Task<(ValidationResult ValidationResult, int? ReviewId)> AddReview(Dtos.Commands.Books.ReviewDto review);

	Task<ValidationResult> EditReview(Dtos.Commands.Books.ReviewDto review);

	Task DeleteReview(int reviewId);

	Task DeleteBook(int bookId);
}