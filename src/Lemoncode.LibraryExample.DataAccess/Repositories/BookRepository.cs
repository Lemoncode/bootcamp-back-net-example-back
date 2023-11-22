using AutoMapper;
using AutoMapper.QueryableExtensions;

using Lemoncode.LibraryExample.DataAccess.Context;
using Lemoncode.LibraryExample.DataAccess.Extensions.EntityExtensions;
using Lemoncode.LibraryExample.DataAccess.Repositories.Helpers;
using Lemoncode.LibraryExample.Domain.Abstractions.Entities;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Entities;
using Lemoncode.LibraryExample.Domain.Exceptions;

using Microsoft.EntityFrameworkCore;

using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;

namespace Lemoncode.LibraryExample.DataAccess.Repositories;

public class BookRepository : IBookRepository
{
	private readonly LibraryDbContext _context;

	private readonly IPaginationHelper _paginationHelper;

	private readonly IMapper _mapper;

	public BookRepository(LibraryDbContext context, IPaginationHelper paginationHelper, IMapper mapper)
	{
		_context = context;
		_paginationHelper = paginationHelper;
		_mapper = mapper;
	}

	public async Task<IEnumerable<Book>> GetNovelties(int limit) =>
		await _context.Books
			.OrderByDescending(b => b.Created)
			.Take(limit)
			.ProjectTo<Book>(_mapper.ConfigurationProvider)
			.ToListAsync();

	public async Task<IEnumerable<Book>> Search(string text) =>
		await _context.Books
			.Where(b =>
			b.Title.Contains(text)
			|| b.Authors.Any(a => (a.FirstName.Contains(text) || a.LastName.Contains(text))))
			.ProjectTo<Book>(_mapper.ConfigurationProvider)
		.ToListAsync();

	public async Task<IEnumerable<Book>> GetTopRatedBooks() =>
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

	public async Task<bool> BookExists(int bookId)
	{
		return await _context.Books.AnyAsync(b => b.Id == bookId);
	}

	public async Task<IIdentifiable> AddBook(AddOrEditBook book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));
		var dalBook = _mapper.Map<DalEntities.Book>(book);
		/* Idealmente, no deberíamos sobrepasar los límites de este aggregate root (libros) para obtener los autores,
		 * pero en EF, si las entidades no están en seguimiento (tracked), EF considerará que los autores son nuevos.
		 * Una opción sería crear autores proxies, usando solo el identificador, pero puesto que nuestras entidades
		 * están usando el modificador required, no podemos hacer esto.
		 * Por tanto, siendo pragmáticos y por mor de la sencillez, en ocasiones hacemos concesiones al DDD puro
		 * y nos saltamos los límites del agregado de libros para poder obtener los autores desde el contexto
		 * y adjuntarlo a nuestro nuevo libro.
		 * */
		var authors = await _context.Authors.Where(a => book.AuthorIds.Contains(a.Id)).ToListAsync();
		
		if (authors.Count != book.AuthorIds.Length)
		{
			throw new EntityNotFoundException($"One or more authors don't exist in the database.");
		}

		dalBook.Authors = authors;
		_context.Books.Add(dalBook);
		return dalBook;
	}

	public async Task EditBook(int bookId, AddOrEditBook book)
	{
		var existingBook = await _context.Books.FindAsync(bookId);
		if (existingBook is null)
		{
			throw new EntityNotFoundException($"The book with identifier {bookId} does not exist in the database.");
		}

		var authors = await _context.Authors.Where(a => book.AuthorIds.Contains(a.Id)).ToListAsync();
		if (authors.Count != book.AuthorIds.Length)
		{
			throw new EntityNotFoundException($"One or more authors don't exist in the database.");
		}
		
		existingBook.Authors = authors;
		existingBook.UpdateDalBook(book);
		existingBook.Authors = authors;
	}

	public async Task DeleteBook(int bookId)
	{
		var book = await _context.Books.FindAsync(bookId);
		if (book is null)
		{
			throw new EntityNotFoundException($"Unable to find a book with ID {bookId}.");
		}

		_context.Books.Remove(book);
	}
}
