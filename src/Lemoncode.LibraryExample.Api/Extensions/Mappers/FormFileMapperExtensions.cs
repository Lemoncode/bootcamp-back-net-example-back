using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Microsoft.AspNetCore.Http;

namespace Lemoncode.LibraryExample.Api.Extensions.Mappers;

internal static class FormFileMapperExtensions
{

	public static async Task<BookImageUploadDto> ConvertToBookUploadImageDto(this IFormFile formFile)
	{
		var memoryStream = new MemoryStream();
		var fileStream = formFile.OpenReadStream();
		await fileStream.CopyToAsync(memoryStream);
		memoryStream.Seek(0, SeekOrigin.Begin);

		return new BookImageUploadDto
		{
			FileName = formFile.FileName,
			ContentType = formFile.ContentType,
			Length = formFile.Length,
			BinaryData = memoryStream
		};
	}
}


