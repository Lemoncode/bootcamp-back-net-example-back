using Lemoncode.LibraryExample.Application.Dtos.Queries.Authors;

namespace Lemoncode.LibraryExample.Application.Abstractions.Queries;

public interface IAuthorQueriesService
{

	Task<List<AuthorWithBookCountDto>> GetAuthors();
	Task<AuthorWithBookCountDto> GetAuthorById(int authorId);
}
