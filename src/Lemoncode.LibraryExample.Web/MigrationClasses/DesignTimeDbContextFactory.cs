using Lemoncode.LibraryExample.DataAccess.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lemoncode.LibraryExample.Web;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
{
	public LibraryDbContext CreateDbContext(string[] args)
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.AddUserSecrets(typeof(Program).Assembly)
			.Build();
		var builder = new DbContextOptionsBuilder<LibraryDbContext>();
		var connectionString = configuration.GetConnectionString("DefaultConnectionString");
		builder.UseSqlServer(connectionString);
		return new LibraryDbContext(builder.Options);
	}
}