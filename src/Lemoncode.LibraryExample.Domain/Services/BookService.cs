using AutoMapper;

using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Abstractions.Services;
using Lemoncode.LibraryExample.Domain.Entities;
using Lemoncode.LibraryExample.Domain.Exceptions;

namespace Lemoncode.LibraryExample.Domain.Services;

public class BookService : IBookService
{

	private readonly IBookRepository _bookRepository;

	private readonly IUnitOfWork _unitOfWork;
	
	private readonly IAuthorRepository _authorRepository;


	public BookService(IBookRepository repository, IUnitOfWork unitOfWork, IAuthorRepository authorRepository)
	{
		_bookRepository = repository;
		_unitOfWork = unitOfWork;
		_authorRepository = authorRepository;
	}

	public async Task<IEnumerable<Book>> GetMostDownloadedBooksAsync()
	{
		return await _bookRepository.GetMostDownloadedBooksAsync();
	}

	public async Task<IEnumerable<Book>> GetNovelties(int limit)
	{
		return await _bookRepository.GetNovelties(limit);
	}

	public async Task<IEnumerable<Book>> GetTopRatedBooks()
	{
		return await _bookRepository.GetTopRatedBooks();
	}

	public async Task<IEnumerable<Book>> Search(string text)
	{
		return await _bookRepository.Search(text);
	}

	public async Task<int> AddBook(AddOrEditBook book)
	{
		if (!await _authorRepository.AuthorsExist(book.AuthorIds))
		{
			throw new EntityNotFoundException($"One or more authors don't exist in the database.");
		}

		var identifiableEntity = await _bookRepository.AddBook(book);
		await _unitOfWork.CommitAsync();
		return identifiableEntity.Id;
	}

	public async Task EditBook(int bookId, AddOrEditBook book)
	{
		if (!await _bookRepository.BookExists(bookId))
		{
			throw new EntityNotFoundException($"The book with ID {bookId} does not exist in the database.");
		}

		if (!await _authorRepository.AuthorsExist(book.AuthorIds))
		{
			throw new EntityNotFoundException($"One or more authors don't exist in the database.");
		}

		await _bookRepository.EditBook(bookId, book);
		await _unitOfWork.CommitAsync();
	}

	public async Task DeleteBook(int bookId)
	{
		if (!await _bookRepository.BookExists(bookId))
		{
			throw new EntityNotFoundException($"The book with ID {bookId} does not exist in the database.");
		}

		await _bookRepository.DeleteBook(bookId);
		await _unitOfWork.CommitAsync();
	}
}
