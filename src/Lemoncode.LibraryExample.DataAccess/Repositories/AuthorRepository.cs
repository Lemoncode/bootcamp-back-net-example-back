using AutoMapper;
using AutoMapper.QueryableExtensions;

using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.DataAccess.Context;
using Lemoncode.LibraryExample.DataAccess.Repositories.Helpers;
using Lemoncode.LibraryExample.Domain.Entities;

namespace Lemoncode.LibraryExample.DataAccess.Repositories;

public class AuthorRepository
{

	private readonly LibraryDbContext _context;

	private readonly PaginationHelper _paginationHelper;

	private readonly IMapper _mapper;

	public AuthorRepository(LibraryDbContext context, PaginationHelper paginationHelper, IMapper mapper)
	{
		_context = context;
		_paginationHelper = paginationHelper;
		_mapper = mapper;
	}

	public Task<PaginatedResults<AuthorWithBookCount>> GetAuthors(int pageNumber, int resultsPerPage)
	{
		return _paginationHelper.PaginateIQueryableAsync(_context.Authors.OrderBy(a => a.FirstName).OrderBy(a => a.LastName)
			.ProjectTo<AuthorWithBookCount>(_mapper.ConfigurationProvider),
			pageNumber, resultsPerPage);
	}
}
