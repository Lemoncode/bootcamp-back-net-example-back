using Lemoncode.LibraryExample.Api.Extensions;
using Lemoncode.LibraryExample.Api.Extensions.Mappers;
using Lemoncode.LibraryExample.Application.Abstractions.Queries;
using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace Lemoncode.LibraryExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{

	private readonly IBookService _bookService;

	private readonly IBookQueriesService _bookQueriesService;

	public BooksController(IBookService bookService, IBookQueriesService bookQueriesService)
	{
		_bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
		_bookQueriesService = bookQueriesService ?? throw new ArgumentNullException(nameof(bookQueriesService));
	}

	[HttpGet("{bookId}/image")]
	public async Task<IActionResult> GetBookImage(int bookId)
	{
		try
		{
			var imageInfo = await _bookQueriesService.GetBookImage(bookId);
			return File(imageInfo.BinaryData, imageInfo.ContentType);
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}

	[HttpGet("{bookId}")]
	public async Task<IActionResult> GetBook([FromRoute] int bookId)
	{
		try
		{
			return Ok(await _bookQueriesService.GetBook(bookId));
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}

	[HttpGet("Novelties")]
	public async Task<IActionResult> GetNovelties([FromQuery] int limit)
	{
		try
		{
			return Ok(await _bookQueriesService.GetNoveltiesAsync(limit));
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}

	[HttpPost("newImage")]
	public async Task<IActionResult> UploadBookImage(IFormFile file)
	{
		ArgumentNullException.ThrowIfNull(file, nameof(file));

		var operationInfo = await _bookService.UploadBookImage(await file.ConvertToBookUploadImageDto());
		if (!operationInfo.ValidationResult.IsValid)
		{
			operationInfo.ValidationResult.AddToModelState(this.ModelState);
			return this.ValidationProblem();
		}
		return Ok(new { Id = operationInfo.ImageUri!.ToString() });
	}

	[HttpPost("")]
	public async Task<IActionResult> AddBook(BookDto book)
	{
		var operationInfo = await _bookService.AddBook(book);
		if (!operationInfo.ValidationResult.IsValid)
		{
			operationInfo.ValidationResult.AddToModelState(this.ModelState);
			return this.ValidationProblem();
		}
		return Created($"/api/books/{operationInfo.book}", book);
	}

	[HttpPut("{bookId}")]
	public async Task<IActionResult> EditBook([FromRoute] int bookId, BookDto book)
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
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}

	[HttpDelete("{bookId}")]
	public async Task<IActionResult> DeleteBook([FromRoute] int bookId)
	{
		try
		{
			await _bookService.DeleteBook(bookId);
			return NoContent();
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}

	[HttpGet("{bookId}/reviews")]
	public async Task<IActionResult> GetReviews(
	[FromRoute, Required] int bookId,
	[FromQuery, Range(1, int.MaxValue, ErrorMessage = "The page number must be greater than 1.")] int page = 1,
	[FromQuery, Range(1, int.MaxValue, ErrorMessage = "The page size must be greater than 1.")] int pageSize = 10)
	{
		try
		{
			return Ok(await _bookQueriesService.GetReviews(bookId, page, pageSize));
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}

	[HttpGet("{bookId}/reviews/{reviewId}")]
	public async Task<IActionResult> GetReview([FromRoute]int bookId, [FromRoute]int reviewId)
	{
		try
		{
			return Ok(await _bookQueriesService.GetReview(bookId, reviewId));
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}


	[HttpPost("{bookId}/reviews")]
	public async Task<IActionResult> AddReview([FromRoute]int bookId, [FromBody]ReviewDto review)
	{
		var operationInfo = await _bookService.AddReview(review, bookId);
		if (!operationInfo.ValidationResult.IsValid)
		{
			operationInfo.ValidationResult.AddToModelState(this.ModelState);
			return ValidationProblem();
		}

		review.Id = operationInfo.ReviewId!.Value;
		return Created($"/api/books/{bookId}/reviews/{review.Id}", review);
	}

	[HttpPut("{bookId}/reviews")]
	public async Task<IActionResult> EditReview([FromRoute]int bookId, [FromBody]ReviewDto review)
	{
		try
		{
			var validationResult = await _bookService.EditReview(review, bookId);
			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(this.ModelState);
				return ValidationProblem();
			}

			return Ok(review);
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}

	[HttpDelete("{bookId}/reviews/{reviewId}")]
	public async Task<IActionResult> Delete([FromRoute]int bookId, [FromRoute]int reviewId)
	{
		try
		{
			await _bookService.DeleteReview(bookId, reviewId);
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}

		return NoContent();
	}
}
