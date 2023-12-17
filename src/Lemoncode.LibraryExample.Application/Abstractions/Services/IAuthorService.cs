using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Dtos.Authors;
using Lemoncode.LibraryExample.Application.Dtos.Books;
using Lemoncode.LibraryExample.Crosscutting;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services
{
	public interface IAuthorService
	{
		Task<PaginatedResults<AuthorWithBookCountDto>> GetAuthors(int pageNumber, int pageSize);

		Task<AuthorWithBookCountDto> GetAuthor(int authorId);

		Task<(ValidationResult ValidationResult, int? AuthorId)> AddAuthor(AuthorDto author);

		Task<ValidationResult> EditAuthor(AuthorDto author);

		Task DeleteAuthor(int authorId);
	}
}