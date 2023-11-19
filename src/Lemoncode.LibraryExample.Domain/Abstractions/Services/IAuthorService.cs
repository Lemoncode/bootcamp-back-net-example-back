using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.Domain.Entities;

namespace Lemoncode.LibraryExample.Domain.Abstractions.Services;

public interface IAuthorService
{
	Task<PaginatedResults<AuthorWithBookCount>> GetAuthors(int pageNumber, int pageSize);

	Task<int> AddAuthor(Author author);
}