using Lemoncode.LibraryExample.DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.DataAccess.EntityTypeConfigurations
{
	public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
	{
		
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.Property(p => p.Name)
				.HasMaxLength(100)
				.IsRequired(true);
			builder.Property(p => p.LastName)
				.HasMaxLength(100)
				.IsRequired(true);
			builder.Property(p => p.Email)
				.HasMaxLength(100)
				.IsRequired(true);
		}
	}
}
