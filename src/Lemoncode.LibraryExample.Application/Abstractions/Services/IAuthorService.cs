using FluentValidation.Results;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Authors;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
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