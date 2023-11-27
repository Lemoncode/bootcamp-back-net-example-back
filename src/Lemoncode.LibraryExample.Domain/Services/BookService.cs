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

	public async Task<int> AddBook(AddOrEditBook book)
	{
		_addOrEditBookValidator.ValidateAndThrow(book);
		
		if (!await _authorRepository.AuthorsExist(book.AuthorIds))
		{
			throw new EntityNotFoundException($"One or more authors don't exist in the database.");
		}
		
		if (!_bookImageRepository.BookImageExists(book.TempImageFileName))
		{
			throw new FileNotFoundException("Unable to find the temporary file of the book image. Please retry the image upload and re-sendthe book.");
		}
		
		var identifiableEntity = await _bookRepository.AddBook(book);
		_bookImageRepository.AssignImageToBook(identifiableEntity.Id, book.TempImageFileName);
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
