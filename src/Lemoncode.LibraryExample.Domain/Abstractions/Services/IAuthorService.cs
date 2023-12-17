using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.Domain.Entities;
using Lemoncode.LibraryExample.Domain.Entities.Authors;

namespace Lemoncode.LibraryExample.Domain.Abstractions.Services;

public interface IAuthorService
{
	Task<PaginatedResults<AuthorWithBookCount>> GetAuthors(int pageNumber, int pageSize);
	
	Task<AuthorWithBookCount> GetAuthor(int authorId);

	Task<int> AddAuthor(Author author);

	Task EditAuthor(Author author);

	Task DeleteAuthor(int authorId);
}