using FluentValidation;

using Lemoncode.LibraryExample.Application.Config.Validators;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Validators.Books
{
	public class BookImageUploadDtoValidator : AbstractValidator<BookImageUploadDto>
	{

		public BookImageUploadDtoValidator(IOptionsSnapshot<BookImageUploadDtoValidatorConfig> config)
		{
			RuleFor(x => x.Length)
				.Must(l => l / 1024 < config.Value.ImageMaxSizeInKBytes)
				.WithMessage($"The book cover image cannot exceed {config.Value.ImageMaxSizeInKBytes} KB.");
			RuleFor(x => x.ContentType)
				.Must(x => config.Value.SupportedImageTypes.Contains(x))
				.WithMessage($"The image type is not supported. We only support {string.Join(", ", config.Value.SupportedImageTypes)}.");
		}
	}
}
