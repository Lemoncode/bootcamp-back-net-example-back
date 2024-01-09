using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;

using Microsoft.AspNetCore.Http;

namespace Lemoncode.LibraryExample.Application.Extensions.Mappers;

internal static class FormFileMapperExtensions
{

	public static BookImageUploadDto ConvertToBookUploadImageDto(this IFormFile formFile)
	{
		return new BookImageUploadDto
		{
			FileName = formFile.FileName,
			ContentType = formFile.ContentType,
			Length = formFile.Length,
			BinaryData = formFile.OpenReadStream()
		};
	}
}


