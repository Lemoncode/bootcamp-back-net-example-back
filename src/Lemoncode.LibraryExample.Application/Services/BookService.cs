using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Books;
using Lemoncode.LibraryExample.Application.Validators.Books;
using Lemoncode.LibraryExample.Domain.Entities.Books;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MimeDetective;

using DomServices = Lemoncode.LibraryExample.Domain.Abstractions.Services;

namespace Lemoncode.LibraryExample.Application.Services;

public class BookService : IBookService
{

	private readonly DomServices.IBookService _bookDomainService;

	private readonly IMapper _mapper;

	private readonly IValidator<BookImageUploadDto> _bookImageUploadDtoValidator;

	private readonly IValidator<AddOrEditBookDto> _AddOrEditBookDtoValidator;

	private readonly ContentInspector _contentInspector;

	public BookService(DomServices.IBookService bookDomainService, IMapper mapper, IValidator<BookImageUploadDto> bookImageUploadDtoValidator, IValidator<AddOrEditBookDto> addOrEditBookDtoValidator, ContentInspector contentInspector)
	{
		_bookDomainService = bookDomainService;
		_mapper = mapper;
		_bookImageUploadDtoValidator = bookImageUploadDtoValidator;
		_AddOrEditBookDtoValidator = addOrEditBookDtoValidator;
		_contentInspector = contentInspector;
	}

	public FileStreamResult GetBookImage(int bookId)
	{
		var imageInfo = _bookDomainService.GetBookImage(bookId);
		var mimeResult = _contentInspector.Inspect(imageInfo.Stream);
		var contentType = !mimeResult.Any() ? "application/octet-stream" : mimeResult[0].Definition.File.MimeType;
		imageInfo.Stream.Seek(0, SeekOrigin.Begin);
		var result = new FileStreamResult(imageInfo.Stream, contentType)
		{
			FileDownloadName = imageInfo.FileName
		};

		return result;
	}

	public async Task<BookDto> GetBook(int bookId)
	{
		return _mapper.Map<BookDto>(await _bookDomainService.GetBook(bookId));
	}

	public async Task<IEnumerable<BookDto>> GetMostDownloadedBooksAsync()
	{
		var result = await _bookDomainService.GetMostDownloadedBooksAsync();
		return _mapper.Map<IEnumerable<BookDto>>(result);
	}

	public async Task<IEnumerable<BookDto>> GetNoveltiesAsync(int limit)
	{
		var result = await _bookDomainService.GetNovelties(limit);
		return _mapper.Map<IEnumerable<BookDto>>(result);
	}

	public async Task<IEnumerable<BookDto>> GetTopRatedBooksAsync()
	{
		var result = await _bookDomainService.GetTopRatedBooks();
		return _mapper.Map<IEnumerable<BookDto>>(result);
	}

	public async Task<IEnumerable<BookDto>> SearchByTitleAsync(string text)
	{
		var result = await _bookDomainService.Search(text);
		return _mapper.Map<IEnumerable<BookDto>>(result);
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
			imageId = await _bookDomainService.UploadBookImage(bookImageUpload);
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
				await _bookDomainService.AddBook(_mapper.Map<AddOrEditBook>(book))));
	}

	public async Task<ValidationResult> EditBook(AddOrEditBookDto book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));

		book.Operation = AddOrEditBookDto.OperationType.Edit;

		var validationResult = await _AddOrEditBookDtoValidator.ValidateAsync(book);
		if (validationResult.IsValid)
		{
			await _bookDomainService.EditBook(_mapper.Map<AddOrEditBook>(book));
		}

		return validationResult;
	}

	public Task DeleteBook(int bookId)
	{
		return _bookDomainService.DeleteBook(bookId);
	}
}
