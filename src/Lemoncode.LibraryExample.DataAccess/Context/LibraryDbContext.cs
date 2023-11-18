using Lemoncode.LibraryExample.Domain.Entities;

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

	public DbSet<BookDownload> BookDownloads { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
	}
	public DbSet<User> Users { get; set; }
}
