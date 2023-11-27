﻿using Lemoncode.LibraryExample.Api.Extensions;
using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Books;

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

	[HttpPost("uploadBookImage")]
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

	[HttpPost("")]
	public async Task<IActionResult> AddBook(AddOrEditBookDto book)
	{
		ArgumentNullException.ThrowIfNull(book, nameof(book));

		var operationInfo = await _bookService.AddBook(book);
		if (!operationInfo.ValidationResult.IsValid)
		{
			operationInfo.ValidationResult.AddToModelState(this.ModelState);
			return this.ValidationProblem();
		}
		return Ok(book);
	}

}
