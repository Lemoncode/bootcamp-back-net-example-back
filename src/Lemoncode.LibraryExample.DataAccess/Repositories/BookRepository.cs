using AutoMapper;
using AutoMapper.QueryableExtensions;

using Lemoncode.LibraryExample.DataAccess.Context;
using Lemoncode.LibraryExample.DataAccess.Repositories.Helpers;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Entities;
using DalEntities=Lemoncode.LibraryExample.DataAccess.Entities;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lemoncode.LibraryExample.DataAccess.Exceptions;

namespace Lemoncode.LibraryExample.DataAccess.Repositories;

public class BookRepository : IBookRepository
{
	private readonly LibraryDbContext _context;

	private readonly PaginationHelper _paginationHelper;

	private readonly IMapper _mapper;

	public BookRepository(LibraryDbContext context, PaginationHelper paginationHelper, IMapper mapper)
	{
		_context = context;
		_paginationHelper = paginationHelper;
		_mapper = mapper;
	}

	public async Task<IEnumerable<Book>> GetNoveltiesAsync(int limit) =>
		await _context.Books
			.OrderByDescending(b => b.Created)
			.Take(limit)
			.ProjectTo<Book>(_mapper.ConfigurationProvider)
			.ToListAsync();

	public async Task<IEnumerable<Book>> SearchByTitleAsync(string text) =>
		await _context.Books
			.Where(b =>
			b.Title.Contains(text)
			|| b.Authors.Any(a => (a.FirstName.Contains(text) || a.LastName.Contains(text))))
			.ProjectTo<Book>(_mapper.ConfigurationProvider)
		.ToListAsync();

	public async Task<IEnumerable<Book>> GetTopRatedBooksAsync() =>
		await _context.Books
			.Where(b => b.Reviews.Any())
			.OrderByDescending(b => b.Reviews.Average(r => r.Stars))
			.Take(10)
			.ProjectTo<Book>(_mapper.ConfigurationProvider)
			.ToListAsync();

	public async Task<IEnumerable<Book>> GetMostDownloadedBooksAsync() =>
		await _context.Books
			.OrderByDescending(b => b.Downloads.Count)
			.Take(10)
			.ProjectTo<Book>(_mapper.ConfigurationProvider)
			.ToListAsync();

	public void AddBook(Book book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));
		var dalBook = _mapper.Map<DalEntities.Book>(book);
		_context.Books.Add(dalBook);
		book.Id = dalBook.Id;
	}

	public async Task DeleteBook(int bookId)
	{
		var book = await _context.Books.FindAsync(bookId);
		if (book == null)
		{ 
			throw new EntityNotFoundException($"Unable to find a book with ID {bookId}.");
		}
		
		_context.Books.Remove(book);
	}
}
