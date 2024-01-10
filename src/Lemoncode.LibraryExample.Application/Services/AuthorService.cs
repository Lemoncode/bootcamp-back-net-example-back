using FluentValidation;
using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Authors;
using Lemoncode.LibraryExample.Application.Extensions.Mappers;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Entities.Authors;

namespace Lemoncode.LibraryExample.Application.Services;

public class AuthorService : IAuthorService
{
	private readonly IValidator<AuthorDto> _authorDtoValidator;

	private IAuthorRepository _authorRepository;
	private readonly IUnitOfWork _unitOFWork;

	
	public AuthorService(IValidator<AuthorDto> authorDtoValidator, IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
	{
		_authorDtoValidator = authorDtoValidator;
		_authorRepository = authorRepository;
		_unitOFWork = unitOfWork;
	}

	public async Task<(ValidationResult ValidationResult, int? AuthorId)> AddAuthor(AuthorDto author)
	{
		var validationResult = _authorDtoValidator.Validate(author);
		if (validationResult.IsValid)
		{
			var repoResult = await _authorRepository.AddAuthor(author.ConvertToDomainEntity());
			await _unitOFWork.CommitAsync();

			return (validationResult, repoResult.Id);
		}
		return (validationResult, null);
	}

	public async Task<ValidationResult> EditAuthor(AuthorDto author)
	{
		var validationResult = _authorDtoValidator.Validate(author);
		if (validationResult.IsValid)
		{
			Author authorEntity = null;

			try
			{
				authorEntity = await _authorRepository.GetAuthor(author.Id);
			}
			catch (Domain.Exceptions.EntityNotFoundException ex)
			{
				throw new Application.Exceptions.EntityNotFoundException($"Unable to find an author with id {author.Id}.", ex);
			}

			authorEntity.UPdateFirstName(author.FirstName);
			authorEntity.UpdateLastName(author.LastName);

			await _authorRepository.EditAuthor(authorEntity);
			await _unitOFWork.CommitAsync();
		}

		return validationResult;
	}

	public async Task DeleteAuthor(int authorId)
	{
		await _authorRepository.DeleteAuthor(authorId);
		await _unitOFWork.CommitAsync();
	}
}
