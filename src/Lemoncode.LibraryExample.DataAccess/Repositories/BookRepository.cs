using AutoMapper;
using AutoMapper.QueryableExtensions;

using Lemoncode.LibraryExample.DataAccess.Context;
using Lemoncode.LibraryExample.DataAccess.Repositories.Helpers;
using Lemoncode.LibraryExample.Domain.Abstractions.Entities;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Entities;
using Lemoncode.LibraryExample.Domain.Entities.Books;
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

	public async Task<Book> GetBook(int bookId) =>
		_mapper.Map<Book>(await _context.Books
			.Include(b => b.Authors)
			.SingleOrDefaultAsync(b => b.Id == bookId));

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

	public async Task<IIdentifiable> AddBook(Book book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));
		var dalBook = _mapper.Map<DalEntities.Book>(book);
		
		var authors = await _context.Authors.Where(a => book.Authors.Contains(a.Id)).ToListAsync();

		if (authors.Count != book.Authors.Count())
		{
			throw new EntityNotFoundException($"One or more authors don't exist in the database.");
		}

		dalBook.Authors = authors;
		_context.Books.Add(dalBook);
		return dalBook;
	}

	public async Task EditBook(Book book)
	{
		var existingBook = await _context.Books.Include(b => b.Authors)
			.SingleAsync(b => b.Id == book.Id);
		
		if (existingBook is null)
		{
			throw new EntityNotFoundException($"The book with identifier {book.Id} does not exist in the database.");
		}

		var authors = await _context.Authors.Where(a => book.AuthorIds.Contains(a.Id)).ToListAsync();
		if (authors.Count != book.AuthorIds.Length)
		{
			throw new EntityNotFoundException($"One or more authors don't exist in the database.");
		}

		_mapper.Map(book, existingBook);
		existingBook.Authors.Clear();
		foreach (var author in authors)
		{
			existingBook.Authors.Add(author);
		}
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

	public Task<bool> ReviewExists(int reviewId)
	{
		return _context.Reviews.AnyAsync(r => r.Id == reviewId);
	}

	public Task<IIdentifiable> AddReview(Review review)
	{
		var dalReview = _mapper.Map<DalEntities.Review>(review);
		_context.Reviews.Add(dalReview);
		return Task.FromResult((IIdentifiable)dalReview);
	}

	public async Task EditReview(Review review)
	{
		var reviewFromDb = await _context.Reviews.FindAsync(review.Id);
		if (reviewFromDb is null)
		{
			throw new EntityNotFoundException($"The review with ID {review.Id} was not found.");
		}

		_mapper.Map(review, reviewFromDb);
	}

	public async Task DeleteReview(int reviewId)
	{
		var reviewFromDb = await _context.Reviews.FindAsync(reviewId);
		if (reviewFromDb is null)
		{
			throw new EntityNotFoundException($"The author with ID {reviewId} was not found.");
		}

		_context.Reviews.Remove(reviewFromDb);
	}
}
