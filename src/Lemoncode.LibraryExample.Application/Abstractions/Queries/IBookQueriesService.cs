using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;
using Lemoncode.LibraryExample.Application.Queries.Pagination;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Abstractions.Queries
{
	public interface IBookQueriesService
	{

		Task<BookImageUploadDto> GetBookImage(int bookId);
		
		Task<BookDto> GetBook(int bookId);
		Task<IList<BookDto>> GetNoveltiesAsync(int limit);
	}
}
