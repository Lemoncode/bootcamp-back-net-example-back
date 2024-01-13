using FluentValidation;

using Lemoncode.LibraryExample.Application.Dtos.Commands.Authors;

namespace Lemoncode.LibraryExample.Application.Validators.Authors;

public class AuthorValidator : AbstractValidator<AuthorDto>
{
	public AuthorValidator()
	{
		RuleFor(p => p.FirstName)
			.NotNull()
			.NotEmpty()
			.Length(1, 100)
			.WithMessage("The author first name should contains between 1 and 100 characters.");

		RuleFor(p => p.LastName)
			.NotNull()
			.NotEmpty()
			.Length(1, 100)
			.WithMessage("The author first name should contains between 1 and 100 characters.");
	}
}
