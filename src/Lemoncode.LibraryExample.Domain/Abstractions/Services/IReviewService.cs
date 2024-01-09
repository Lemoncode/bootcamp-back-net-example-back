using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.Domain.Entities.Authors;
using Lemoncode.LibraryExample.Domain.Entities.Books;

namespace Lemoncode.LibraryExample.Domain.Abstractions.Services;

public interface IReviewService
{
	Task<PaginatedResults<Review>> GetReviews(int bookId, int pageNumber, int pageSize);
	
	Task<Review> GetReview(int reviewId);

	Task<int> AddReview(AddOrEditReview review);

	Task EditReview(AddOrEditReview review);

	Task DeleteReview(int reviewId);
}