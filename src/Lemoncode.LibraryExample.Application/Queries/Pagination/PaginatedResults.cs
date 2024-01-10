using System.Collections.Generic;

namespace Lemoncode.LibraryExample.Application.Queries.Pagination;

public class PaginatedResults<TItem>
{

	public PaginatedResults(IEnumerable<TItem> results, int currentPage, int resultsPerPage, int totalRows)
	{
		Results = results;
		var metadata = new PaginatedResultsMetadata
		{
			CurrentPage = currentPage,
			ResultsPerPage = resultsPerPage,
			TotalRows = totalRows,
			TotalPages = totalRows / resultsPerPage
		};
		if (totalRows % resultsPerPage != 0)
		{
			metadata.TotalPages++;
		}
		PaginationInfo = metadata;
	}

	public IEnumerable<TItem> Results { get; }

	public PaginatedResultsMetadata PaginationInfo { get; }

}
