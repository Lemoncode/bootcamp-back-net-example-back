﻿using FluentValidation;

using Lemoncode.LibraryExample.Domain.Entities.Books;

namespace Lemoncode.LibraryExample.Domain.Entities.Validators.Books;

public class AddEditBookValidator : AbstractValidator<AddOrEditBook>
{

	public AddEditBookValidator()
	{
		RuleFor(p => p.Title).NotNull().NotEmpty()
			.Length(1, 500)
			.WithMessage("The title should contains between 1 and 500 characters.");

		RuleFor(p => p.AuthorIds)
			.Must(p => p.Length > 0)
			.WithMessage("The book should contains at least one author.");

		RuleFor(p => p.Description).NotNull().NotEmpty()
			.Length(10, 10000)
			.WithMessage("The description should contains between 10 and 10000 characters.");

		RuleFor(p => p.ImageAltText).NotNull().NotEmpty()
			.Length(10, 1000)
			.WithMessage("The alternate text should contains between 10 and 1000 characters.");


		RuleFor(p => p.TempImageFileName)
		.NotNull().NotEmpty().When(e => e.Operation == AddOrEditBook.OperationType.Add)
		.WithMessage("If the operation is an addition, the temp image file cannot be null or empty.");
	}
}
