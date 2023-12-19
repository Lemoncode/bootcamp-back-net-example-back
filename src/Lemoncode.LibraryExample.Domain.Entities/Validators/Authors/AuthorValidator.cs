using FluentValidation;

using Lemoncode.LibraryExample.Domain.Entities.Authors;

namespace Lemoncode.LibraryExample.Domain.Entities.Validators.Authors;

public class AuthorValidator : AbstractValidator<Author>
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
			.WithMessage("The author last name should contains between 1 and 100 characters.");
	}
}
