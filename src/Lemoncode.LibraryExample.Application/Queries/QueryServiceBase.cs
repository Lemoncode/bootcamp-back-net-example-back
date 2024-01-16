using Lemoncode.LibraryExample.Application.Config;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Lemoncode.LibraryExample.Application.Queries;

public abstract class QueryServiceBase : IDisposable
{

	private readonly SqlConnection _connection;
	private bool _disposedValue;

	protected SqlConnection SqlConnection => _connection;

	public QueryServiceBase(IOptionsSnapshot<DapperConfig> dapperConfig)
	{
		ArgumentNullException.ThrowIfNull(dapperConfig, nameof(DapperConfig));
		_connection = new SqlConnection(dapperConfig.Value.DefaultConnectionString);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				_connection?.Dispose();
			}

			_disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
