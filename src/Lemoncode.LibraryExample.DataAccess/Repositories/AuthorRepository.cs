using AutoMapper;
using AutoMapper.QueryableExtensions;

using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.DataAccess.Context;
using Lemoncode.LibraryExample.DataAccess.Repositories.Helpers;
using Lemoncode.LibraryExample.Domain.Abstractions.Entities;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Entities.Authors;
using Lemoncode.LibraryExample.Domain.Exceptions;

using Microsoft.EntityFrameworkCore;

using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;

namespace Lemoncode.LibraryExample.DataAccess.Repositories;

public class AuthorRepository : IAuthorRepository
{

	private readonly LibraryDbContext _context;

	private readonly IPaginationHelper _paginationHelper;

	private readonly IMapper _mapper;

	public AuthorRepository(LibraryDbContext context, IPaginationHelper paginationHelper, IMapper mapper)
	{
		_context = context;
		_paginationHelper = paginationHelper;
		_mapper = mapper;
	}

	public Task<PaginatedResults<AuthorWithBookCount>> GetAuthors(int pageNumber, int pageSize)
	{
		return _paginationHelper.PaginateIQueryableAsync(_context.Authors.OrderBy(a => a.FirstName).OrderBy(a => a.LastName)
			.ProjectTo<AuthorWithBookCount>(_mapper.ConfigurationProvider),
			pageNumber, pageSize);
	}

	public async Task<AuthorWithBookCount> GetAuthor(int authorId)
	{
		return _mapper.Map<AuthorWithBookCount>(await _context.Authors.FindAsync(authorId));
	}

	public async Task<Author> GetAuthorById(int authorId)
	{
		var author = await _context.Authors.FindAsync(authorId);
		return _mapper.Map<Author>(author);
	}

	public async Task<bool> AuthorExists(int authorId)
	{
		return await _context.Authors.AnyAsync(a => a.Id == authorId);
	}
	
	public async Task<bool> AuthorsExist(int[] authorIds)
	{
		return (await _context.Authors.CountAsync(a => authorIds.Contains(a.Id))) == authorIds.Length;
	}

	public Task<IIdentifiable> AddAuthor(Author author)
	{
		var dalAuthor = _mapper.Map<DalEntities.Author>(author);
		_context.Authors.Add(dalAuthor);
		return Task.FromResult((IIdentifiable)dalAuthor);
	}

	public async Task EditAuthor(Author author)
	{
		var authorFromDb = await _context.Authors.FindAsync(author.Id);
		if (authorFromDb is null)
		{
			throw new EntityNotFoundException($"The author with ID {author.Id} was not found.");
		}
		
		_mapper.Map(author, authorFromDb);
	}

	public async Task DeleteAuthor(int authorId)
	{
		var authorFromDb = await _context.Authors.FindAsync(authorId);
		if (authorFromDb is null)
		{
			throw new EntityNotFoundException($"The author with ID {authorId} was not found.");
		}
		
		_context.Authors.Remove(authorFromDb);
	}
}
