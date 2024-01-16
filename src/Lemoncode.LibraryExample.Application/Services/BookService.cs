using FluentValidation;
using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Application.Extensions.Mappers;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.FileStorage;

using AppExceptions = Lemoncode.LibraryExample.Application.Exceptions;
using DomExceptions = Lemoncode.LibraryExample.Domain.Exceptions;

namespace Lemoncode.LibraryExample.Application.Services;

public class BookService : IBookService
{

	private readonly IBookRepository _bookRepository;

	private readonly IUnitOfWork _unitOfWork;

	private readonly IFileRepository _fileRepository;

	private readonly IValidator<BookImageUploadDto> _bookImageUploadDtoValidator;

	private readonly IValidator<BookDto> _bookDtoValidator;

	private readonly IValidator<ReviewDto> _reviewDtoValidator;

	public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork, IFileRepository fileRepository, IValidator<BookImageUploadDto> bookImageUploadDtoValidator, IValidator<BookDto> bookDtoValidator, IValidator<ReviewDto> reviewDtoValidator)
	{
		_bookRepository = bookRepository;
		_unitOfWork = unitOfWork;
		_fileRepository = fileRepository;
		_bookImageUploadDtoValidator = bookImageUploadDtoValidator;
		_bookDtoValidator = bookDtoValidator;
		_reviewDtoValidator = reviewDtoValidator;
	}

	public async Task<(ValidationResult ValidationResult, Uri? ImageUri)> UploadBookImage(BookImageUploadDto bookImageUploadDto)
	{
		ArgumentNullException.ThrowIfNull(bookImageUploadDto, nameof(bookImageUploadDto));

		var validationResult = await _bookImageUploadDtoValidator.ValidateAsync(bookImageUploadDto);
		Uri? imageUri = null;

		if (validationResult.IsValid)
		{
			imageUri = await _fileRepository.UploadTempFile(bookImageUploadDto.BinaryData, bookImageUploadDto.FileName);
		}

		return (validationResult, imageUri);
	}

	public async Task<(ValidationResult ValidationResult, int? book)> AddBook(BookDto book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));

		book.Operation = BookDto.OperationType.Add;
		var validationResult = _bookDtoValidator.Validate(book);

		if (!validationResult.IsValid)
		{
			return (validationResult, null);
		}

		var originalImageExtension = Path.GetExtension(book.TempImageFileName);
		var permanentFileName = await _fileRepository.MoveFileToPermanentLocation(new Uri(book.TempImageFileName!), $"{Guid.NewGuid()}{originalImageExtension}");
		var result = await _bookRepository.AddBook(book.ConvertToDomainEntity(permanentFileName.ToString()));
		await _unitOfWork.CommitAsync();
		return (
			validationResult, result.Id);
	}

	public async Task<ValidationResult> EditBook(int bookId, BookDto book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));

		book.Operation = BookDto.OperationType.Edit;

		var validationResult = await _bookDtoValidator.ValidateAsync(book);
		if (validationResult.IsValid)
		{
			var bookEntity = await _bookRepository.GetBook(bookId);

			if (bookEntity is null)
			{
				throw new AppExceptions.EntityNotFoundException($"Unable to find a book with ID {bookId}.");
			}

			bookEntity.UpdateTitle(book.Title);
			bookEntity.UpdateAuthors(book.AuthorIds);

			if (book.TempImageFileName is not null)
			{
				var originalImageExtension = Path.GetExtension(book.TempImageFileName);
				var permanentFileName = await _fileRepository.MoveFileToPermanentLocation(new Uri(book.TempImageFileName!), $"{Guid.NewGuid()}{originalImageExtension}");

				bookEntity.UpdateImage(permanentFileName.ToString(), book.ImageAltText);
			}
			else
			{
				bookEntity.UpdateImage(bookEntity.Image.FileName, book.ImageAltText);
			}

			bookEntity.UpdateDescription(book.Description);
			await _bookRepository.EditBook(bookEntity);
			await _unitOfWork.CommitAsync();
		}

		return validationResult;
	}

	public async Task DeleteBook(int bookId)
	{
		try
		{
			await _bookRepository.DeleteBook(bookId);
			await _unitOfWork.CommitAsync();
		}
		catch (DomExceptions.EntityNotFoundException ex)
		{
			throw new AppExceptions.EntityNotFoundException(ex.Message, ex);
		}
	}

	public async Task<(ValidationResult ValidationResult, int? ReviewId)> AddReview(ReviewDto review, int bookId)
	{
		ArgumentNullException.ThrowIfNull(review, nameof(review));

		var validationResult = _reviewDtoValidator.Validate(review);
		if (!await _bookRepository.BookExists(bookId))
		{
			throw new AppExceptions.EntityNotFoundException($"Unable to find the book with Id {bookId}.");
		}

		if (!validationResult.IsValid)
		{
			return (validationResult, null);
		}

		var result = await _bookRepository.AddReview(review.ConvertToDomainEntity(bookId));
		await _unitOfWork.CommitAsync();

		return (validationResult, result.Id);
	}

	public async Task<ValidationResult> EditReview(ReviewDto review, int bookId)
	{
		ArgumentNullException.ThrowIfNull(review, nameof(review));

		var validationResult = _reviewDtoValidator.Validate(review);
		if (validationResult.IsValid)
		{
			if (!await _bookRepository.BookExists(bookId))
			{
				throw new AppExceptions.EntityNotFoundException($"Unable to find the book with Id {bookId}.");
			}

			var reviewEntity = await _bookRepository.GetReview(review.Id);
			if (reviewEntity is null)
			{
				throw new AppExceptions.EntityNotFoundException($"Unable to find the review with id {review.Id}.");
			}

			reviewEntity.UpdateReviewer(review.Reviewer);
			reviewEntity.UpdateText(review.ReviewText);
			reviewEntity.UpdateStars(review.Stars);

			await _bookRepository.EditReview(reviewEntity);
			await _unitOfWork.CommitAsync();
		}

		return validationResult;
	}

	public async Task DeleteReview(int bookId, int reviewId)
	{
		if (!await _bookRepository.BookExists(bookId))
		{
			throw new AppExceptions.EntityNotFoundException($"Unable to find a book with Id {bookId}.");
		}
		
		try
		{
			await _bookRepository.DeleteReview(reviewId);
			await _unitOfWork.CommitAsync();
		}
		catch (DomExceptions.EntityNotFoundException ex)
		{
			throw new AppExceptions.EntityNotFoundException(ex.Message, ex);
		}
	}
}
