using Lemoncode.LibraryExample.DataAccess.Context;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Entities;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.DataAccess.Repositories;

public class BookRepository : IBookRepository
{
	private readonly LibraryDbContext _context;

	public BookRepository(LibraryDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Book>> GetNoveltiesAsync(int limit) =>
		await _context.Books
			.OrderByDescending(b => b.Created)
			.Take(limit)
			.ToListAsync();

	public async Task<IEnumerable<Book>> SearchByTitleAsync(string text) =>
		await _context.Books
			.Where(b =>
			b.Title.Contains(text)
			|| b.Authors.Any(a => (a.FirstName.Contains(text) || a.LastName.Contains(text))))
			.ToListAsync();

	public async Task<IEnumerable<Book>> GetTopRatedBooksAsync() =>
		await _context.Books
			.Where(b => b.Reviews.Any())
			.OrderByDescending(b => b.Reviews.Average(r => r.Stars))
			.Take(10)
			.ToListAsync();

	public async Task<IEnumerable<Book>> GetMostDownloadedBooksAsync() =>
		await _context.Books
			.OrderByDescending(b => b.Downloads.Count)
			.Take(10)
			.ToListAsync();

	public async Task AddBook(Book book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));
		_context.Books.Add(book);
	}

	public void DeleteBook(Book book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));
		_context.Books.Remove(book);
	}
}
