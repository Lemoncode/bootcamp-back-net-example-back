using Lemoncode.LibraryExample.Api.Extensions;
using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Books;
using Lemoncode.LibraryExample.Domain.Exceptions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Lemoncode.LibraryExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{

	private readonly IBookService _bookService;

	public BooksController(IBookService bookService)
	{
		_bookService = bookService;
	}

	[HttpGet("{bookId}/image")]

	public IActionResult GetBookImage(int bookId)
	{
		return _bookService.GetBookImage(bookId);
	}

	[HttpGet("{bookId}")]
	public async Task<IActionResult> GetBook([FromRoute] int bookId)
	{
		try
		{
			return Ok(await _bookService.GetBook(bookId));
		}
		catch (EntityNotFoundException)
		{
			return NotFound(bookId);
		}
	}

	[HttpGet("Novelties")]
	public async Task<IActionResult> GetNovelties([FromQuery] int limit)
	{
		try
		{
			return Ok(await _bookService.GetNoveltiesAsync(limit));
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, $"Unhandled exception ocurred: {ex.Message}");
		}
	}

	[Authorize]
	[HttpPost("newImage")]
	public async Task<IActionResult> UploadBookImage(IFormFile file)
	{
		ArgumentNullException.ThrowIfNull(file, nameof(file));

		var operationInfo = await _bookService.UploadBookImage(file);
		if (!operationInfo.ValidationResult.IsValid)
		{
			operationInfo.ValidationResult.AddToModelState(this.ModelState);
			return this.ValidationProblem();
		}
		return Ok(new { Id = operationInfo.ImageId });
	}

	[Authorize]
	[HttpPost("")]
	public async Task<IActionResult> AddBook(AddOrEditBookDto book)
	{
		var operationInfo = await _bookService.AddBook(book);
		if (!operationInfo.ValidationResult.IsValid)
		{
			operationInfo.ValidationResult.AddToModelState(this.ModelState);
			return this.ValidationProblem();
		}
		return Created($"/api/books/{operationInfo.book?.Id}" ,operationInfo.book);
	}

	[Authorize]
	[HttpPut("{bookId}")]
	public async Task<IActionResult> EditBook([FromRoute]int bookId, AddOrEditBookDto book)
	{
		book.Id = bookId;
		try
		{
			var validationResult = await _bookService.EditBook(book);
			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(this.ModelState);
				return this.ValidationProblem();
			}
			return Ok(book);
		}
		catch (EntityNotFoundException)
		{
			return NotFound(bookId);
		}
	}

	[Authorize]
	[HttpDelete("{bookId}")]
	public async Task<IActionResult> DeleteBook([FromRoute]int bookId)
	{
		try
		{
			await _bookService.DeleteBook(bookId);
			return NoContent();
		}
		catch (EntityNotFoundException)
		{
			return NotFound(bookId);
		}
	}
}
