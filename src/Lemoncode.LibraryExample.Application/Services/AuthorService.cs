using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Authors;
using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.Domain.Entities.Authors;
using DomServiceAbstractions = Lemoncode.LibraryExample.Domain.Abstractions.Services;
using DomExceptions = Lemoncode.LibraryExample.Domain.Exceptions;
using AppExceptions = Lemoncode.LibraryExample.Application.Exceptions;
using System.Net;

namespace Lemoncode.LibraryExample.Application.Services;

public class AuthorService : IAuthorService
{
	private readonly IValidator<AuthorDto> _authorDtoValidator;

	private readonly DomServiceAbstractions.IAuthorService _authorService;

	private readonly IMapper _mapper;

	public AuthorService(IValidator<AuthorDto> authorDtoValidator, DomServiceAbstractions.IAuthorService authorService, IMapper mapper)
	{
		_authorDtoValidator = authorDtoValidator;
		_authorService = authorService;
		_mapper = mapper;
	}

	public async Task<PaginatedResults<AuthorWithBookCountDto>> GetAuthors(int pageNumber, int pageSize)
	{
		return _mapper.Map<PaginatedResults<AuthorWithBookCountDto>>(await _authorService.GetAuthors(pageNumber, pageSize));
	}

	public async Task<AuthorWithBookCountDto> GetAuthor(int authorId)
	{
		try
		{
			return _mapper.Map<AuthorWithBookCountDto>(await _authorService.GetAuthor(authorId));
		}
		catch(DomExceptions.EntityNotFoundException ex)
		{
			throw new AppExceptions.EntityNotFoundException(ex.Message, HttpStatusCode.NotFound, ex);
		}
	}

	public async Task<(ValidationResult ValidationResult, int? AuthorId)> AddAuthor(AuthorDto author)
	{
		var validationResult = _authorDtoValidator.Validate(author);
		return (validationResult, validationResult.IsValid ?
			await _authorService.AddAuthor(_mapper.Map<Author>(author)) : null);
	}

	public async Task<ValidationResult> EditAuthor(AuthorDto author)
	{
		var validationResult = _authorDtoValidator.Validate(author);
		if (validationResult.IsValid)
		{
			await _authorService.EditAuthor(_mapper.Map<Author>(author));
		}

		return validationResult;
	}

	public Task DeleteAuthor(int authorId)
	{
		return _authorService.DeleteAuthor(authorId);
	}
}
