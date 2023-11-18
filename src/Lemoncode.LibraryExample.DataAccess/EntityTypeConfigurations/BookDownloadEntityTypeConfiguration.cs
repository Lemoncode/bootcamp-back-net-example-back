using Lemoncode.LibraryExample.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lemoncode.LibraryExample.DataAccess.EntityTypeConfigurations;

public class BookDownloadEntityTypeConfiguration : IEntityTypeConfiguration<BookDownload>
{
	public void Configure(EntityTypeBuilder<BookDownload> builder)
	{
		builder.HasKey(p => p.Id);
		builder.HasOne(p => p.Book).WithMany(p => p.Downloads);
		builder.HasOne(p => p.User).WithMany(p => p.BookDownloads);
		builder.Property(p => p.IPAddress)
			.HasMaxLength(39 /*Expanded IP V6 can contains up to 39 characters including separators*/)
			.IsRequired(true);
		builder.Property(p => p.Date)
			.IsRequired(true);
	}
}
