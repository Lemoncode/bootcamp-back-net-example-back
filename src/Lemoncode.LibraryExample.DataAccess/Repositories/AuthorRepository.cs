using AutoMapper;

using Lemoncode.LibraryExample.DataAccess.Context;
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


	private readonly IMapper _mapper;

	public AuthorRepository(LibraryDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}
	public async Task<Author> GetAuthor(int authorId)
	{
		var author = await _context.Authors.SingleOrDefaultAsync(a => a.Id == authorId);
		if (author is null)
		{
			throw new EntityNotFoundException($"Unable to find an author with id {authorId}.");
		}

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
