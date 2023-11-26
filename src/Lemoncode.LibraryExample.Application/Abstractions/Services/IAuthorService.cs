using Lemoncode.LibraryExample.Application.Dtos.Authors;
using Lemoncode.LibraryExample.Crosscutting;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services
{
	public interface IAuthorService
	{
		Task<PaginatedResults<AuthorWithBookCountDto>> GetAuthors(int pageNumber, int pageSize);

		Task<int> AddAuthor(AuthorDto author);
	}
}