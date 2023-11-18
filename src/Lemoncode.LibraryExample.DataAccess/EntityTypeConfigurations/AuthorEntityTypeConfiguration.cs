using Lemoncode.LibraryExample.DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lemoncode.LibraryExample.DataAccess.EntityTypeConfigurations
{
	public class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
	{
		public void Configure(EntityTypeBuilder<Author> builder)
		{
			builder.HasKey(p => p.Id);
			builder.Property(p => p.FirstName)
				.HasMaxLength(100)
				.IsRequired(true);
			builder.Property(p => p.LastName)
				.HasMaxLength(100)
				.IsRequired(true);
		}
	}
}
