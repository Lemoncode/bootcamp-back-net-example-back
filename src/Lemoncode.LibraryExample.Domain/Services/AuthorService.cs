using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Abstractions.Services;
using Lemoncode.LibraryExample.Domain.Entities.Authors;

namespace Lemoncode.LibraryExample.Domain.Services;

public class AuthorService : IAuthorService
{

	private readonly IAuthorRepository _authorRepository;

	private readonly IUnitOfWork _unitOfWork;

	public AuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
	{
		_authorRepository = authorRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<PaginatedResults<AuthorWithBookCount>> GetAuthors(int pageNumber, int pageSize)
	{
		return await _authorRepository.GetAuthors(pageNumber, pageSize);
	}

	public async Task<int> AddAuthor(Author author)
	{
		var identifiableObject = await _authorRepository.AddAuthor(author);
		await _unitOfWork.CommitAsync();
		return identifiableObject.Id;
	}
}
