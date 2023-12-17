using Lemoncode.LibraryExample.DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.DataAccess.EntityTypeConfigurations;

public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
{
	public void Configure(EntityTypeBuilder<Book> builder)
	{
		builder.HasKey(p => p.Id);
		builder.HasMany(p => p.Reviews)
			.WithOne(p => p.Book);
		builder.HasMany(p => p.Authors)
			.WithMany(p => p.Books);
		builder.Property(p => p.Title)
			.HasMaxLength(500)
			.IsRequired(true);
		builder.Property(p => p.Description)
			.HasMaxLength(10000)
			.IsRequired(true);
		builder.Property(p => p.ImageAltText)
			.HasMaxLength(1000)
			.IsRequired(true);
		builder.Property(p => p.AVerage)
			.HasPrecision(2);
	}
}
