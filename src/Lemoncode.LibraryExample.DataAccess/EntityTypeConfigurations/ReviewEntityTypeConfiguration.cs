using Lemoncode.LibraryExample.DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lemoncode.LibraryExample.DataAccess.EntityTypeConfigurations;

public class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
	public void Configure(EntityTypeBuilder<Review> builder)
	{
		builder.ToTable(tbl => tbl.HasTrigger("trg_UpdateBookAverage"));
		builder.HasKey(p => p.Id);
		builder.Property(p => p.ReviewText)
			.HasMaxLength(4000)
			.IsRequired(true);
		builder.Property(p => p.Stars)
			.IsRequired(true);
		builder.Property(p => p.Reviewer)
			.HasMaxLength(100)
			.IsRequired(true);
	}
}
