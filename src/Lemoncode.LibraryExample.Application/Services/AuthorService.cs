using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Authors;
using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.Domain.Entities.Authors;
using DomServiceAbstractions = Lemoncode.LibraryExample.Domain.Abstractions.Services;

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

	public async Task<(ValidationResult ValidationResult, int? BookId)> AddAuthor(AuthorDto author)
	{
		var validationResult = _authorDtoValidator.Validate(author);
		return (validationResult, validationResult.IsValid ?
			await _authorService.AddAuthor(_mapper.Map<Author>(author)) : null);
	}
}
