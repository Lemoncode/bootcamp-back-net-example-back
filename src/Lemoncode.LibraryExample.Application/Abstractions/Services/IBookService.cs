using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services;

public interface IBookService
{
	Task<(ValidationResult ValidationResult, Uri? ImageUri)> UploadBookImage(BookImageUploadDto file);

	Task<(ValidationResult ValidationResult, int? book)> AddBook(Dtos.Commands.Books.BookDto book);

	Task<ValidationResult> EditBook(int bookId, Dtos.Commands.Books.BookDto book);

	Task<(ValidationResult ValidationResult, int? ReviewId)> AddReview(Dtos.Commands.Books.ReviewDto review, int bookId);

	Task<ValidationResult> EditReview(Dtos.Commands.Books.ReviewDto review, int bookId);

	Task DeleteReview(int bookId, int reviewId);

	Task DeleteBook(int bookId);
}