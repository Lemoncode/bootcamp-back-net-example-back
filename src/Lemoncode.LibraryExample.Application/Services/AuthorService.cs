using AutoMapper;
using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos;
using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.Domain.Entities;
using DomServiceAbstractions = Lemoncode.LibraryExample.Domain.Abstractions.Services;

namespace Lemoncode.LibraryExample.Application.Services;

public class AuthorService : IAuthorService
{

	private readonly DomServiceAbstractions.IAuthorService _authorService;

	private readonly IMapper _mapper;

	public AuthorService(DomServiceAbstractions.IAuthorService authorService, IMapper mapper)
	{
		_authorService = authorService;
		_mapper = mapper;
	}

	public async Task<PaginatedResults<AuthorWithBookCountDto>> GetAuthors(int pageNumber, int pageSize)
	{
		return _mapper.Map<PaginatedResults<AuthorWithBookCountDto>>(await _authorService.GetAuthors(pageNumber, pageSize));
	}

	public async Task<int> AddAuthor(AuthorDto author)
	{
		return await _authorService.AddAuthor(_mapper.Map<Author>(author));
	}
}
