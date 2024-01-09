using FluentValidation;
using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Application.Validators.Books;
using Lemoncode.LibraryExample.Domain.Entities.Books;

using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MimeDetective;

namespace Lemoncode.LibraryExample.Application.Services;

public class BookService : IBookService
{

	private readonly IBookRepository _bookRepository;

	private readonly IFileRepository _bookImageRepository;

	private readonly IValidator<BookImageUploadDto> _bookImageUploadDtoValidator;

	private readonly IValidator<AddOrEditBookDto> _AddOrEditBookDtoValidator;

	public BookService(IBookRepository bookRepository, IFileRepository bookImageRepository, IValidator<BookImageUploadDto> bookImageUploadDtoValidator, IValidator<AddOrEditBookDto> addOrEditBookDtoValidator)
	{
		_bookRepository = bookRepository;
		_bookImageRepository = bookImageRepository;
		_bookImageUploadDtoValidator = bookImageUploadDtoValidator;
		_AddOrEditBookDtoValidator = addOrEditBookDtoValidator;
	}


	public async Task<(ValidationResult ValidationResult, string? ImageId)> UploadBookImage(IFormFile file)
	{
		ArgumentNullException.ThrowIfNull(file, nameof(file));

		var bookImageUploadDto = _mapper.Map<BookImageUploadDto>(file);
		var validationResult = await _bookImageUploadDtoValidator.ValidateAsync(bookImageUploadDto);
		string? imageId = null;

		if (validationResult.IsValid)
		{
			var bookImageUpload = _mapper.Map<BookImageUpload>(bookImageUploadDto);
			/* Descargamos el fichero que nos viene del IFormFile a un MemoryStream para poder hacer el Dispose de este Stream
 			*de una manera controlada aquí, y tener un objeto de dominio independiente con la copia de ese stream.
			* Si la imagen fuera muy grande, seguramente tenerla en memoria no sería una buena idea, pero para estas pequeñas imágenes
			* en las que ya hemos definido un tamaño máximo pequeño, es viable.
			*/
			var mStr = new MemoryStream();
			await bookImageUploadDto.BinaryData.CopyToAsync(mStr);
			mStr.Seek(0, SeekOrigin.Begin);
			bookImageUpload.BinaryData = mStr;
			imageId = await _bookImageRepository.UploadImageToTempFile(bookImageUpload);
		}

		/* Desechamos el stream que abrimos al mapear el objeto de IFormFile a BookImageUploadDto.
		 * En el mapeo de BookImageUploadDto a BookImageUpload (entidad de dominio), hemos copiado ese stream a un MemoryStream, por lo que la referencia
		  * al stream de descarga del fichero desde el cliente ya se puede cerrar sin problemas.
		*/
		bookImageUploadDto.Dispose();

		return (validationResult, imageId);
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

		return (
			validationResult,
			_mapper.Map<BookDto>(
				await _bookRepository.AddBook(_mapper.Map<AddOrEditBook>(book))));
	}

	public async Task<ValidationResult> EditBook(AddOrEditBookDto book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));

		book.Operation = AddOrEditBookDto.OperationType.Edit;

		var validationResult = await _AddOrEditBookDtoValidator.ValidateAsync(book);
		if (validationResult.IsValid)
		{
			await _bookRepository.EditBook(_mapper.Map<AddOrEditBook>(book));
		}

		return validationResult;
	}

	public Task DeleteBook(int bookId)
	{
		return _bookRepository.DeleteBook(bookId);
	}
}
