using Lemoncode.LibraryExample.Api.Extensions;
using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Reviews;
using Lemoncode.LibraryExample.Application.Exceptions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Lemoncode.LibraryExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{

	private readonly IReviewService _reviewService;

	public ReviewsController(IReviewService reviewService)
	{
		_reviewService = reviewService;
	}

	[HttpGet("/api/books/{bookId}/reviews")]
	public async Task<IActionResult> GetReviews(
		[FromRoute, Required]int bookId,
		[FromQuery, Range(1, int.MaxValue, ErrorMessage = "The page number must be greater than 1.")] int page = 1,
		[FromQuery, Range(1, int.MaxValue, ErrorMessage = "The page size must be greater than 1.")] int pageSize = 10)
	{
		try
		{
			return Ok(await _reviewService.GetReviews(bookId, page, pageSize));
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}

	[HttpGet("{reviewId}")]
	public async Task<IActionResult> GetReview(int reviewId)
	{
		try
		{
			return Ok(await _reviewService.GetReview(reviewId));
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}


	[Authorize]
	[HttpPost]
	public async Task<IActionResult> AddReview(AddOrEditReviewDto review)
	{
		var operationInfo = await _reviewService.AddReview(review);
		if (!operationInfo.ValidationResult.IsValid)
		{
			operationInfo.ValidationResult.AddToModelState(this.ModelState);
			return ValidationProblem();
		}

		review.Id = operationInfo.ReviewId!.Value;
		return Created($"/api/books/{review.Id}", review);
	}

	[Authorize]
	[HttpPut]
	public async Task<IActionResult> EditReview(AddOrEditReviewDto review)
	{
		try
		{
			var validationResult = await _reviewService.EditReview(review);
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

	[Authorize]
	[HttpDelete("{reviewId}")]
	public async Task<IActionResult> Delete(int reviewId)
	{
		try
		{
			await _reviewService.DeleteReview(reviewId);
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
		
		return NoContent();
	}
}
