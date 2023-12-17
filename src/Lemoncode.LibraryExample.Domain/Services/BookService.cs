using AutoMapper;

using FluentValidation;

using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Abstractions.Services;
using Lemoncode.LibraryExample.Domain.Entities;
using Lemoncode.LibraryExample.Domain.Entities.Books;
using Lemoncode.LibraryExample.Domain.Exceptions;

namespace Lemoncode.LibraryExample.Domain.Services;

public class BookService : IBookService
{

	private readonly IBookRepository _bookRepository;

	private readonly IBookImageRepository _bookImageRepository;
	
	private readonly IUnitOfWork _unitOfWork;

	private readonly IAuthorRepository _authorRepository;

	private readonly IValidator<AddOrEditBook> _addOrEditBookValidator;

	public BookService(IBookRepository repository, IUnitOfWork unitOfWork, IAuthorRepository authorRepository, IBookImageRepository bookImageRepository, IValidator<AddOrEditBook> addOrEditBookValidator)
	{
		_bookRepository = repository;
		_unitOfWork = unitOfWork;
		_authorRepository = authorRepository;
		_bookImageRepository = bookImageRepository;
		_addOrEditBookValidator = addOrEditBookValidator;
	}
	public (Stream Stream, string FileName) GetBookImage(int bookId) =>
		_bookImageRepository.GetBookImage(bookId);

	public async Task<Book> GetBook(int bookId)
	{
		return await _bookRepository.GetBook(bookId);
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

	public async Task<string> UploadBookImage(BookImageUpload bookImageUpload)
	{
		return await _bookImageRepository.UploadImageToTempFile(bookImageUpload);
	}

	public async Task<Book> AddBook(AddOrEditBook book)
	{
		_addOrEditBookValidator.ValidateAndThrow(book);
		
		if (!await _authorRepository.AuthorsExist(book.AuthorIds))
		{
			throw new EntityNotFoundException($"One or more authors don't exist in the database.");
		}
		
		if (!_bookImageRepository.TempImageExists(book.TempImageFileName))
		{
			throw new FileNotFoundException("Unable to find the temporary file of the book image. Please retry the image upload and re-sendthe book.");
		}
		
		book.Created = DateTime.UtcNow;

		var identifiableEntity = await _bookRepository.AddBook(book);
		await _unitOfWork.CommitAsync();
		_bookImageRepository.AssignImageToBook(identifiableEntity.Id, book.TempImageFileName);
		var addedBook = await _bookRepository.GetBook(identifiableEntity.Id);
		return addedBook;
	}

	public async Task EditBook(AddOrEditBook book) 
	{
		if (!await _bookRepository.BookExists(book.Id))
		{
			throw new EntityNotFoundException($"The book with ID {book.Id} does not exist in the database.");
		}

		if (!await _authorRepository.AuthorsExist(book.AuthorIds))
		{
			throw new EntityNotFoundException($"One or more authors don't exist in the database.");
		}

		if (!string.IsNullOrWhiteSpace(book.TempImageFileName))
		{
			if (!_bookImageRepository.TempImageExists(book.TempImageFileName))
			{
				throw new FileNotFoundException("Unable to find the temporary file of the book image. Please retry the image upload and re-sendthe book.");
			}
		}

		await _bookRepository.EditBook(book);
		if (!string.IsNullOrWhiteSpace(book.TempImageFileName))
		{
			_bookImageRepository.AssignImageToBook(book.Id, book.TempImageFileName);
		}

		await _unitOfWork.CommitAsync();
	}

	public async Task DeleteBook(int bookId)
	{
		if (!await _bookRepository.BookExists(bookId))
		{
			throw new EntityNotFoundException($"The book with ID {bookId} does not exist in the database.");
		}

		await _bookRepository.DeleteBook(bookId);
		_bookImageRepository.DeleteImage(bookId);
		await _unitOfWork.CommitAsync();
	}
}
