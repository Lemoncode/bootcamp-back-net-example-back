﻿using FluentValidation;

using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Abstractions.Services;
using Lemoncode.LibraryExample.Domain.Entities.Authors;

namespace Lemoncode.LibraryExample.Domain.Services;

public class AuthorService : IAuthorService
{

	private readonly IAuthorRepository _authorRepository;

	private readonly IUnitOfWork _unitOfWork;

	private readonly IValidator<Author> _authorValidator;
	
	public AuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork, IValidator<Author> authorValidator)
	{
		_authorRepository = authorRepository;
		_unitOfWork = unitOfWork;
		_authorValidator = authorValidator;
	}

	public async Task<PaginatedResults<AuthorWithBookCount>> GetAuthors(int pageNumber, int pageSize)
	{
		return await _authorRepository.GetAuthors(pageNumber, pageSize);
	}

	public async Task<int> AddAuthor(Author author)
	{
		_authorValidator.ValidateAndThrow(author);

		var identifiableObject = await _authorRepository.AddAuthor(author);
		await _unitOfWork.CommitAsync();
		return identifiableObject.Id;
	}
}
