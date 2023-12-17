using Lemoncode.LibraryExample.Api.Extensions;
using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Authors;
using Lemoncode.LibraryExample.Domain.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace Lemoncode.LibraryExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{

	private readonly IAuthorService _authorService;

	public AuthorsController(IAuthorService authorService)
	{
		_authorService = authorService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAuthors(
		[FromQuery, Range(1, int.MaxValue, ErrorMessage = "The page number must be greater than 1.")] int page = 1,
		[FromQuery, Range(1, int.MaxValue, ErrorMessage = "The page size must be greater than 1.")] int pageSize = 10)
	{
		return Ok(await _authorService.GetAuthors(page, pageSize));
	}


	[HttpGet("{authorId}")]
	public async Task<IActionResult> GetAuthor(int authorId)
	{
		try
		{
			return Ok(await _authorService.GetAuthor(authorId));
		}
		catch (EntityNotFoundException)
		{
			return NotFound();
		}
	}


	[HttpPost]
	public async Task<IActionResult> AddAuthor(AuthorDto author)
	{
		var operationInfo = await _authorService.AddAuthor(author);
		if (!operationInfo.ValidationResult.IsValid)
		{
			operationInfo.ValidationResult.AddToModelState(this.ModelState);
			return ValidationProblem();
		}

		author.Id = operationInfo.AuthorId!.Value;
		return Created($"/api/books/{author.Id}", author);
	}

	[HttpPut]
	public async Task<IActionResult> EditAuthor(AuthorDto author)
	{
		try
		{
			var validationResult = await _authorService.EditAuthor(author);
			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(this.ModelState);
				return ValidationProblem();
			}

			return Ok(author);
		}
		catch (EntityNotFoundException)
		{
			return NotFound();
		}
	}

	[HttpDelete("{authorId}")]
	public async Task<IActionResult> Delete(int authorId)
	{
		try
		{
			await _authorService.DeleteAuthor(authorId);
		}
		catch (EntityNotFoundException)
		{
			return NotFound();
		}
		
		return NoContent();
	}
}
