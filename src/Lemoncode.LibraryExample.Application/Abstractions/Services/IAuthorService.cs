using FluentValidation.Results;
using Lemoncode.LibraryExample.Application.Dtos.Books;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Authors;
using Lemoncode.LibraryExample.Crosscutting;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services
{
	public interface IAuthorService
	{
		Task<(ValidationResult ValidationResult, int? AuthorId)> AddAuthor(AuthorDto author);

		Task<ValidationResult> EditAuthor(AuthorDto author);

		Task DeleteAuthor(int authorId);
	}
}