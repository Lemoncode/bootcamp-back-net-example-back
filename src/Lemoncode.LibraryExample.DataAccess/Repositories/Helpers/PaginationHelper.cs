using Lemoncode.LibraryExample.Crosscutting;

using Microsoft.EntityFrameworkCore;

namespace Lemoncode.LibraryExample.DataAccess.Repositories.Helpers;

public class PaginationHelper : IPaginationHelper
{

	public void CheckPaginationValidity(int resultsPerPage, int pageNumber)
	{
		if (pageNumber <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "The param must be greater than zero.");
		}
		if (resultsPerPage <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(resultsPerPage), resultsPerPage, "the parameter mus be treater than zero.");
		}
	}

	public int CalculateOffset(int resultsPerPage, int pageNumber)
	{
		return (pageNumber - 1) * resultsPerPage;
	}

	public async Task<PaginatedResults<TEntity>> PaginateIQueryableAsync<TEntity>(IQueryable<TEntity> entities, int pageNumber, int resultsPerPage)
	{
		var totalRows = await entities.CountAsync();
		var paginatedRows = await entities.Skip((pageNumber - 1) * resultsPerPage).Take(resultsPerPage).ToListAsync();
		return new PaginatedResults<TEntity>(paginatedRows, pageNumber, resultsPerPage, totalRows);
	}
}
