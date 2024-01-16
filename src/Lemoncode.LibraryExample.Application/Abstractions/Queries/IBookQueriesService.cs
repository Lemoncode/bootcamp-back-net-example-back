using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;
using Lemoncode.LibraryExample.Application.Queries.Pagination;

namespace Lemoncode.LibraryExample.Application.Abstractions.Queries;

public interface IBookQueriesService
{

	Task<BookImageUploadDto> GetBookImage(int bookId);

	Task<BookDto> GetBook(int bookId);

	Task<IList<BookDto>> GetNoveltiesAsync(int limit);

	Task<PaginatedResults<ReviewDto>> GetReviews(int bookId, int page, int pageSize);

	Task<ReviewDto> GetReview(int bookId, int reviewId);
}
