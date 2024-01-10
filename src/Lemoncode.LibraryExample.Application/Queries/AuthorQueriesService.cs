using Dapper;

using Lemoncode.LibraryExample.Application.Abstractions.Queries;
using Lemoncode.LibraryExample.Application.Config;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Authors;
using Lemoncode.LibraryExample.Application.Exceptions;
using Lemoncode.LibraryExample.Application.Queries.Pagination;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Lemoncode.LibraryExample.Application.Queries;

public class AuthorQueriesService : QueryServiceBase, IAuthorQueriesService
{

	public AuthorQueriesService(IOptionsSnapshot<DapperConfig> dapperConfig) : base(dapperConfig)
	{
	}

	public async Task<PaginatedResults<AuthorWithBookCountDto>> GetAuthors(int pageNumber = 1, int resultsPerPage = 10)
	{
		PaginationHelper.CheckPaginationValidity(pageNumber, resultsPerPage);
		var totalAuthors = await SqlConnection.QuerySingleOrDefaultAsync<int>("SELECT COUNT(1) FROM AUTHORS");

		var result = await PaginationHelper.PaginateAsync(totalAuthors, pageNumber, resultsPerPage, async (offset, fetch) =>
		{
			return await SqlConnection.QueryAsync<AuthorWithBookCountDto>(
			@"SELECT
				    a.Id,
				a.FirstName,
				a.LastName,
				COUNT(ab.BooksId) as BookCount
				FROM
				Authors a
				LEFT JOIN 
				AuthorBook ab ON a.Id = ab.AuthorsId
				GROUP BY 
				a.Id, a.FirstName, a.LastName
				ORDER BY a.FirstName, a.LastName
				OFFSET @offset ROWS
				FETCH NEXT @fetch ROWS ONLY",
				new { offset = offset, fetch = fetch });
		});

		return result;
	}

	public async Task<AuthorWithBookCountDto> GetAuthorById(int authorId)
	{
		var author = await SqlConnection.QuerySingleOrDefaultAsync<AuthorWithBookCountDto>(
			@"SELECT
				    a.Id,
				a.FirstName,
				a.LastName,
				COUNT(ab.BooksId) as BookCount
				FROM
				Authors a
				LEFT JOIN 
				AuthorBook ab ON a.Id = ab.AuthorsId
				WHERE a.Id=@authorId
				GROUP BY 
				a.Id, a.FirstName, a.LastName", new { authorId = authorId });

		if (author is null)
		{
			throw new EntityNotFoundException($"Unable to find the author with ID {authorId}.");
		}

		return author;
	}
}