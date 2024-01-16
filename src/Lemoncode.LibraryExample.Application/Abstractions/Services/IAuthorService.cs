using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Dtos.Commands.Authors;

namespace Lemoncode.LibraryExample.Application.Abstractions.Services
{
	public interface IAuthorService
	{
		Task<(ValidationResult ValidationResult, int? AuthorId)> AddAuthor(AuthorDto author);

		Task<ValidationResult> EditAuthor(AuthorDto author);

		Task DeleteAuthor(int authorId);
	}
}