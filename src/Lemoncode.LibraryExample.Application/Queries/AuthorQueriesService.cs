using Dapper;

using Lemoncode.LibraryExample.Application.Abstractions.Queries;
using Lemoncode.LibraryExample.Application.Config;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Authors;
using Lemoncode.LibraryExample.Application.Exceptions;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Lemoncode.LibraryExample.Application.Queries
{
	public class AuthorQueriesService : IAuthorQueriesService, IDisposable
	{

		private readonly SqlConnection _connection;
		private bool disposedValue;

		public AuthorQueriesService(IOptionsSnapshot<DapperConfig> dapperConfig)
		{
			_connection = new SqlConnection(dapperConfig.Value.DefaultConnectionString);
			_connection.Open();
		}

		public async Task<List<AuthorWithBookCountDto>> GetAuthors()
		{
			return (await _connection.QueryAsync<AuthorWithBookCountDto>(
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
				a.Id, a.FirstName, a.LastName")).ToList();
		}

		public async Task<AuthorWithBookCountDto> GetAuthorById(int authorId)
		{
			var author = await _connection.QuerySingleOrDefaultAsync<AuthorWithBookCountDto>(
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

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_connection.Close();
					_connection.Dispose();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
