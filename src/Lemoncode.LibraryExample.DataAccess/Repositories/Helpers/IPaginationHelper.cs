using Lemoncode.LibraryExample.Crosscutting;

namespace Lemoncode.LibraryExample.DataAccess.Repositories.Helpers
{
	public interface IPaginationHelper
	{
		int CalculateOffset(int resultsPerPage, int pageNumber);
		void CheckPaginationValidity(int resultsPerPage, int pageNumber);
		Task<PaginatedResults<TEntity>> PaginateIQueryableAsync<TEntity>(IQueryable<TEntity> entities, int pageNumber, int resultsPerPage);
	}
}