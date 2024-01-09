using FluentValidation;
using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Authors;
using Lemoncode.LibraryExample.Application.Extensions.Mappers;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;

namespace Lemoncode.LibraryExample.Application.Services;

public class AuthorService : IAuthorService
{
	private readonly IValidator<AuthorDto> _authorDtoValidator;

	private IAuthorRepository _authorRepository;
	
	public AuthorService(IValidator<AuthorDto> authorDtoValidator, IAuthorRepository authorRepository)
	{
		_authorDtoValidator = authorDtoValidator;
		_authorRepository = authorRepository;
	}

	public async Task<(ValidationResult ValidationResult, int? AuthorId)> AddAuthor(AuthorDto author)
	{
		var validationResult = _authorDtoValidator.Validate(author);
		return (validationResult, validationResult.IsValid ?
			(await _authorRepository.AddAuthor(author.ConvertToDomainEntity())).Id : null);
	}

	public async Task<ValidationResult> EditAuthor(AuthorDto author)
	{
		var validationResult = _authorDtoValidator.Validate(author);
		if (validationResult.IsValid)
		{
			await _authorRepository.EditAuthor(author.ConvertToDomainEntity());
		}

		return validationResult;
	}

	public Task DeleteAuthor(int authorId)
	{
		return _authorRepository.DeleteAuthor(authorId);
	}
}
