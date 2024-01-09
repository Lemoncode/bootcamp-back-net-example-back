using FluentValidation;
using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Domain.Entities.Authors;

namespace Lemoncode.LibraryExample.Application.Validators.Reviews;

public class AddOrEditReviewDtoValidator : AbstractValidator<AddOrEditReviewDto>
{
	public AddOrEditReviewDtoValidator()
	{
		RuleFor(p => p.ReviewText)
			.NotNull()
			.NotEmpty()
			.Length(10, 4000)
			.WithMessage("The text of the review should contains between 10 and 4000 characters.");

		RuleFor(p => p.Reviewer)
			.NotNull()
			.NotEmpty()
			.Length(3, 100)
			.WithMessage("The reviewer name should contains between 3 and 100 characters.");
		RuleFor(p => p.Stars)
			.Custom((value, context) =>
			{
				if (value < 1 || value > 5)
				{
					context.AddFailure("Stars", "The number of starts should be in the range 1 - 5.");
				}
			});
	}
}
