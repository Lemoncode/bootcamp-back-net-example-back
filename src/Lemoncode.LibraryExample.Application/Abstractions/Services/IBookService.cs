using FluentValidation.Results;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services;

public interface IBookService
{
	Task<(ValidationResult ValidationResult, Uri? ImageUri)> UploadBookImage(IFormFile file);

	Task<(ValidationResult ValidationResult, BookDto? book)> AddBook(AddOrEditBookDto book);

	Task<ValidationResult>  EditBook(AddOrEditBookDto book);

	Task<(ValidationResult ValidationResult, int? ReviewId)> AddReview(AddOrEditReviewDto review);

	Task<ValidationResult> EditReview(AddOrEditReviewDto review);

	Task DeleteReview(int reviewId);
}
Task DeleteBook(int bookId);
}