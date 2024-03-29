﻿using Lemoncode.LibraryExample.Api.Extensions;
using Lemoncode.LibraryExample.Application.Abstractions.Queries;
using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Authors;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace Lemoncode.LibraryExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{

	private readonly IAuthorService _authorService;

	private readonly IAuthorQueriesService _authorQueriesService;

	public AuthorsController(IAuthorService authorService, IAuthorQueriesService authorQueriesService)
	{
		_authorService = authorService;
		_authorQueriesService = authorQueriesService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAuthors(
		[FromQuery, Range(1, int.MaxValue, ErrorMessage = "The page number must be greater than 1.")] int page = 1,
		[FromQuery, Range(1, int.MaxValue, ErrorMessage = "The page size must be greater than 1.")] int pageSize = 10)
	{
		return Ok(await _authorQueriesService.GetAuthors(page, pageSize));
	}


	[HttpGet("{authorId}")]
	public async Task<IActionResult> GetAuthor([FromRoute] int authorId)
	{
		try
		{
			return Ok(await _authorQueriesService.GetAuthorById(authorId));
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}


	[Authorize]
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

	[Authorize]
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
		catch (Exception ex)
		{
			return this.Problem(ex);
		}
	}

	[Authorize]
	[HttpDelete("{authorId}")]
	public async Task<IActionResult> Delete(int authorId)
	{
		try
		{
			await _authorService.DeleteAuthor(authorId);
		}
		catch (Exception ex)
		{
			return this.Problem(ex);
		}

		return NoContent();
	}
}
