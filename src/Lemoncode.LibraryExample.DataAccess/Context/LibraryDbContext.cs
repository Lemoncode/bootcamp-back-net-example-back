using Lemoncode.LibraryExample.DataAccess.Entities;

using Microsoft.EntityFrameworkCore;

namespace Lemoncode.LibraryExample.DataAccess.Context;

public class LibraryDbContext : DbContext
{

	public LibraryDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Book> Books { get; set; }

	public DbSet<Author> Authors { get; set; }

	public DbSet<Review> Reviews { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
	}
}
