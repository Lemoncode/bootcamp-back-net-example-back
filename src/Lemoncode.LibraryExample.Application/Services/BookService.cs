using AutoMapper;

using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Books;
using Lemoncode.LibraryExample.Application.Validators.Books;
using Lemoncode.LibraryExample.Domain.Entities.Books;
using Microsoft.AspNetCore.Http;

using DomServices = Lemoncode.LibraryExample.Domain.Abstractions.Services;

namespace Lemoncode.LibraryExample.Application.Services;

public class BookService : IBookService
{

	private readonly DomServices.IBookService _bookDomainService;

	private readonly IMapper _mapper;

	private readonly BookImageUploadDtoValidator _bookImageUploadDtoValidator;

	public BookService(DomServices.IBookService bookDomainService, IMapper mapper, BookImageUploadDtoValidator bookImageUploadDtoValidator)
	{
		_bookDomainService = bookDomainService;
		_mapper = mapper;
		_bookImageUploadDtoValidator = bookImageUploadDtoValidator;
	}

	public Task<int> AddBook(AddOrEditBookDto book)
	{
		return _bookDomainService.AddBook(_mapper.Map<AddOrEditBook>(book));
	}

	public Task EditBook(int bookId, AddOrEditBookDto book)
	{
		return _bookDomainService.EditBook(bookId, _mapper.Map<AddOrEditBook>(book));
	}

	public Task DeleteBook(int bookId)
	{
		return _bookDomainService.DeleteBook(bookId);
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

	public async Task<(ValidationResult ValidationResult, string ImageId)> UploadBookImage(IFormFile file)
	{
		ArgumentNullException.ThrowIfNull(file, nameof(file));

		var bookImageUploadDto = _mapper.Map<BookImageUploadDto>(file);
		var validationResult = await _bookImageUploadDtoValidator.ValidateAsync(bookImageUploadDto);
		return (validationResult, "abcd");
	}
}
