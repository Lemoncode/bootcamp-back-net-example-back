using Lemoncode.LibraryExample.Application.Dtos.Queries.Authors;
using Lemoncode.LibraryExample.Application.Queries.Pagination;

namespace Lemoncode.LibraryExample.Application.Abstractions.Queries;

public interface IAuthorQueriesService
{

	Task<PaginatedResults<AuthorWithBookCountDto>> GetAuthors(int pageNumber = 1, int resultsPerPage = 10);
	Task<AuthorWithBookCountDto> GetAuthorById(int authorId);
}
