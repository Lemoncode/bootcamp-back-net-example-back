using FluentValidation;

using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Abstractions.Services;
using Lemoncode.LibraryExample.Domain.Entities.Reviews;
using Lemoncode.LibraryExample.Domain.Exceptions;

namespace Lemoncode.LibraryExample.Domain.Services;

public class ReviewService : IReviewService
{

	private readonly IReviewRepository _reviewRepository;

	private readonly IBookRepository _bookRepository;

	private readonly IUnitOfWork _unitOfWork;

	private readonly IValidator<AddOrEditReview> _reviewValidator;

	public ReviewService(IReviewRepository reviewRepository, IBookRepository bookRepository, IUnitOfWork unitOfWork, IValidator<AddOrEditReview> reviewValidator)
	{
		_reviewRepository = reviewRepository;
		_bookRepository = bookRepository;
		_unitOfWork = unitOfWork;
		_reviewValidator = reviewValidator;
	}

	public async Task<PaginatedResults<Review>> GetReviews(int bookId, int pageNumber, int pageSize)
	{
		return await _reviewRepository.GetReviews(bookId, pageNumber, pageSize);
	}

	public async Task<Review> GetReview(int reviewId)
	{
		return await _reviewRepository.GetReviewById(reviewId);
	}

	public async Task<int> AddReview(AddOrEditReview review)
	{
		_reviewValidator.ValidateAndThrow(review);

		if (!await _bookRepository.BookExists(review.BookId))
		{
			throw new EntityNotFoundException($"The book with id {review.BookId} was not found.");
		}

		var identifiableObject = await _reviewRepository.AddReview(review);
		await _unitOfWork.CommitAsync();
		return identifiableObject.Id;
	}

	public async Task EditReview(AddOrEditReview review)
	{
		_reviewValidator.ValidateAndThrow(review);

		var existingReview = await _reviewRepository.GetReviewById(review.Id);
		if (existingReview is null)
		{
			throw new EntityNotFoundException($"The review with id {review.Id} was not found.");
		}

		if (!await _bookRepository.BookExists(review.BookId))
		{
			throw new EntityNotFoundException($"The book with id {review.BookId} was not found.");
		}

		await _reviewRepository.EditReview(review);
		await _unitOfWork.CommitAsync();
	}

	public async Task DeleteReview(int reviewId)
	{
		if (!await _reviewRepository.ReviewExists(reviewId))
		{
			throw new EntityNotFoundException($"The review with id {reviewId} was not found.");
		}

		await _reviewRepository.DeleteReview(reviewId);
		await _unitOfWork.CommitAsync();
	}
}
