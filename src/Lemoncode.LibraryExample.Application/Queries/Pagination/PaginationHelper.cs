namespace Lemoncode.LibraryExample.Application.Queries.Pagination;

public static class PaginationHelper
{

	public static void CheckPaginationValidity(int pageNumber, int resultsPerPage)
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

	public static int CalculateOffset(int resultsPerPage, int pageNumber) => (pageNumber - 1) * resultsPerPage;

	public static async Task<PaginatedResults<TEntity>> PaginateAsync<TEntity>(int totalRows,
																			int pageNumber,
																			int resultsPerPage,
																			Func<int, int, Task<IEnumerable<TEntity>>> queryFunc)
	{
		var offset = CalculateOffset(resultsPerPage, pageNumber);
		return new PaginatedResults<TEntity>(await queryFunc(offset, resultsPerPage), pageNumber, resultsPerPage, totalRows);
	}
}
