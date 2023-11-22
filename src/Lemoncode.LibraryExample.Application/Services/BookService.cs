using AutoMapper;

using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos;
using Lemoncode.LibraryExample.Domain.Entities;
using DomServices = Lemoncode.LibraryExample.Domain.Abstractions.Services;

namespace Lemoncode.LibraryExample.Application.Services;

public class BookService : IBookService
{

	private readonly DomServices.IBookService _bookDomainService;

	private readonly IMapper _mapper;

	public BookService(DomServices.IBookService bookDomainService, IMapper mapper)
	{
		_bookDomainService = bookDomainService;
		_mapper = mapper;
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
}
