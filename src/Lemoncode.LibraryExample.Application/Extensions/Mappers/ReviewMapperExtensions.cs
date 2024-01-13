using Lemoncode.LibraryExample.Application.Dtos.Commands.Books;
using Lemoncode.LibraryExample.Domain.Entities.Books;

namespace Lemoncode.LibraryExample.Application.Extensions.Mappers;

internal static class ReviewMapperExtensions
{

	public static Review ConvertToDomainEntity(this ReviewDto review)
	{
		return new Review(review.Id, review.BookId, review.Reviewer, new ReviewText(review.ReviewText), review.Stars);
	}
}
