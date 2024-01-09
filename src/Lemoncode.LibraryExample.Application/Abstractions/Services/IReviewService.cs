using FluentValidation.Results;
using Lemoncode.LibraryExample.Application.Dtos.Books;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;
using Lemoncode.LibraryExample.Crosscutting;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services
{
	public interface IReviewService
	{
		Task<PaginatedResults<ReviewDto>> GetReviews(int bookId, int pageNumber, int pageSize);

		Task<ReviewDto> GetReview(int reviewId);

		Task<(ValidationResult ValidationResult, int? ReviewId)> AddReview(AddOrEditReviewDto review);

		Task<ValidationResult> EditReview(AddOrEditReviewDto review);

		Task DeleteReview(int reviewId);
	}
}