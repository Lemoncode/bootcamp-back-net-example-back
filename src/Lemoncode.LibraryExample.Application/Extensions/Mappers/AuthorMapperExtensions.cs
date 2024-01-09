using Lemoncode.LibraryExample.Application.Dtos.Queries.Authors;
using Lemoncode.LibraryExample.Domain.Entities.Authors;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Extensions.Mappers;

internal static class AuthorMapperExtensions
{

	public static AuthorDto ConverToDto(this Author author)
	{
		return new AuthorDto
		{
			FirstName = author.FirstName,
			Id = author.Id,
			LastName = author.LastName
		};
	}

	public static Author ConvertToDomainEntity(this AuthorDto authorDto)
	{
		return new Author(
			firstName: authorDto.FirstName,
			id: authorDto.Id,
			lastName: authorDto.LastName);
	}
}
