using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Authors;
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

	[HttpPost]
	public async Task<IActionResult> AddAuthor(AuthorDto author)
	{
		try
		{
			var id = await _authorService.AddAuthor(author);
			author.Id = id;
		return Ok(author);
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
		}
	}
}
