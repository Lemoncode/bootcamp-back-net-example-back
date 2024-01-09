using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Domain.Entities.Books;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Extensions.Mappers;

internal static class ReviewMapperExtensions
{

	public static Review ConvertToDomainEntity(this AddOrEditReviewDto review)
	{
		return new Review(review.Id, review.BookId, review.Reviewer, new ReviewText(review.ReviewText), review.Stars);
	}
}
