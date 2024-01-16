namespace Lemoncode.LibraryExample.Application.Queries.Pagination;

public class PaginatedResultsMetadata
{

	public int CurrentPage { get; set; }

	public int ResultsPerPage { get; internal set; }

	public int TotalPages { get; internal set; }

	public int TotalRows { get; internal set; }

	public bool HasPreviousPage => CurrentPage > 1;

	public bool HasNextPage => CurrentPage < TotalPages;

	public int ResultsFrom =>
		TotalRows == 0 ? 0 :
		((CurrentPage - 1) * ResultsPerPage) + 1;

	public int ResultsTo
	{
		get
		{
			if (TotalRows == 0)
			{
				return 0;
			}

			var to = ResultsFrom + ResultsPerPage - 1;
			if ((to - 1) + ResultsPerPage > TotalRows)
			{
				to = TotalRows;
			}
			return to;
		}
	}
}
