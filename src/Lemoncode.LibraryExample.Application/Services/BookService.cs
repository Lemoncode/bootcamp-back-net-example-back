using FluentValidation;
using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;
using Lemoncode.LibraryExample.Application.Exceptions;
using Lemoncode.LibraryExample.Application.Extensions.Mappers;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.FileStorage;

using Microsoft.AspNetCore.Http;

using System.Runtime.InteropServices;

namespace Lemoncode.LibraryExample.Application.Services;

public class BookService : IBookService
{

	private readonly IBookRepository _bookRepository;

	private readonly IUnitOfWork _unitOfWork;
	
	private readonly IFileRepository _fileRepository;

	private readonly IValidator<BookImageUploadDto> _bookImageUploadDtoValidator;

	private readonly IValidator<AddOrEditBookDto> _AddOrEditBookDtoValidator;
	
	private readonly IValidator<AddOrEditReviewDto> _AddOrEditReviewDtoValidator;

	public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork, IFileRepository fileRepository, IValidator<BookImageUploadDto> bookImageUploadDtoValidator, IValidator<AddOrEditBookDto> addOrEditBookDtoValidator, IValidator<AddOrEditReviewDto> addOrEditReviewDtoValidator)
	{
		_bookRepository = bookRepository;
		_unitOfWork = unitOfWork;
		_fileRepository = fileRepository;
		_bookImageUploadDtoValidator = bookImageUploadDtoValidator;
		_AddOrEditBookDtoValidator = addOrEditBookDtoValidator;
		_AddOrEditReviewDtoValidator = addOrEditReviewDtoValidator;
	}

	public async Task<(ValidationResult ValidationResult, Uri? ImageUri)> UploadBookImage(IFormFile file)
	{
		ArgumentNullException.ThrowIfNull(file, nameof(file));

		var bookImageUploadDto = file.ConvertToBookUploadImageDto();
		var validationResult = await _bookImageUploadDtoValidator.ValidateAsync(bookImageUploadDto);
		Uri? imageUri = null;

		if (validationResult.IsValid)
		{
			/* Descargamos el fichero que nos viene del IFormFile a un MemoryStream para poder hacer el Dispose de este Stream
 			*de una manera controlada aquí, y tener un objeto de dominio independiente con la copia de ese stream.
			* Si la imagen fuera muy grande, seguramente tenerla en memoria no sería una buena idea, pero para estas pequeñas imágenes
			* en las que ya hemos definido un tamaño máximo pequeño, es viable.
			*/
			var mStr = new MemoryStream();
			await bookImageUploadDto.BinaryData.CopyToAsync(mStr);
			mStr.Seek(0, SeekOrigin.Begin);
			imageUri = await _fileRepository.UploadTempFile(mStr, bookImageUploadDto.FileName);
		}

		/* Desechamos el stream que abrimos al mapear el objeto de IFormFile a BookImageUploadDto.
		 * En el mapeo de BookImageUploadDto a BookImageUpload (entidad de dominio), hemos copiado ese stream a un MemoryStream, por lo que la referencia
		  * al stream de descarga del fichero desde el cliente ya se puede cerrar sin problemas.
		*/
		bookImageUploadDto.Dispose();

		return (validationResult, imageUri);
	}

	public async Task<(ValidationResult ValidationResult, BookDto? book)> AddBook(AddOrEditBookDto book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));

		book.Operation = AddOrEditBookDto.OperationType.Add;
		var validationResult = _AddOrEditBookDtoValidator.Validate(book);

		if (!validationResult.IsValid)
		{
			return (validationResult, null);
		}
		
		await _bookRepository.AddBook(book.ConvertToDomainEntity(book.TempImageFileName!, DateTime.UtcNow, DateTime.UtcNow));
		return (
			validationResult, null);
	}

	public async Task<ValidationResult> EditBook(AddOrEditBookDto book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));

		book.Operation = AddOrEditBookDto.OperationType.Edit;

		var validationResult = await _AddOrEditBookDtoValidator.ValidateAsync(book);
		if (validationResult.IsValid)
		{
			var bookEntity = await _bookRepository.GetBook(book.Id);
			if (bookEntity is null)
			{
				throw new EntityNotFoundException($"Unable to find a book with ID {book.Id}.");
			}

			bookEntity.UpdateTitle(book.Title);
			bookEntity.UpdateAuthors(book.AuthorIds);
			
			if (book.TempImageFileName is not null)
			{
				bookEntity.UpdateImage(book.TempImageFileName!, book.ImageAltText);
			}

			bookEntity.UpdateDescription(book.Description);
			await _bookRepository.EditBook(bookEntity);
		}

		return validationResult;
	}

	public Task DeleteBook(int bookId)
	{
		return _bookRepository.DeleteBook(bookId);
	}

	public async Task<(ValidationResult ValidationResult, int? ReviewId)> AddReview(AddOrEditReviewDto review)
	{
		var validationResult = _reviewDtoValidator.Validate(review);
		return (validationResult, validationResult.IsValid ?
			await _reviewService.AddReview(_mapper.Map<AddOrEditReview>(review)) : null);
	}

	public async Task<ValidationResult> EditReview(AddOrEditReviewDto review)
	{
		var validationResult = _reviewDtoValidator.Validate(review);
		if (validationResult.IsValid)
		{
			await _reviewService.EditReview(_mapper.Map<AddOrEditReview>(review));
		}

		return validationResult;
	}

	public async Task DeleteReview(int reviewId)
	{
		try
		{
			await _reviewService.DeleteReview(reviewId);
		}
		catch (DomExceptions.EntityNotFoundException ex)
		{
			throw new AppExceptions.EntityNotFoundException(ex.Message, ex);
		}
	}
}
