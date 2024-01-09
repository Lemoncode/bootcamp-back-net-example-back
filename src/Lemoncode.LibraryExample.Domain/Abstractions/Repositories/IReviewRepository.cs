using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.Domain.Abstractions.Entities;
using Lemoncode.LibraryExample.Domain.Entities.Authors;
using Lemoncode.LibraryExample.Domain.Entities.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Domain.Abstractions.Repositories;

public interface IReviewRepository
{

	Task<PaginatedResults<Review>> GetReviews(int bookId, int pageNumber, int pageSize);
	
	Task<Review> GetReviewById(int reviewId);

	Task<bool> ReviewExists(int reviewId);
	
	Task<IIdentifiable> AddReview(AddOrEditReview review);

	Task EditReview(AddOrEditReview review);

	Task DeleteReview(int reviewId);
}
