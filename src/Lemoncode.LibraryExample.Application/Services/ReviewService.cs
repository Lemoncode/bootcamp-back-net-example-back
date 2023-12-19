using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.Crosscutting;

using DomServiceAbstractions = Lemoncode.LibraryExample.Domain.Abstractions.Services;
using DomExceptions = Lemoncode.LibraryExample.Domain.Exceptions;
using AppExceptions = Lemoncode.LibraryExample.Application.Exceptions;
using Lemoncode.LibraryExample.Domain.Entities.Reviews;
using Lemoncode.LibraryExample.Application.Dtos.Reviews;
using System.Net;
namespace Lemoncode.LibraryExample.Application.Services;

public class ReviewService : IReviewService
{
	private readonly IValidator<AddOrEditReviewDto> _reviewDtoValidator;

	private readonly DomServiceAbstractions.IReviewService _reviewService;

	private readonly IMapper _mapper;

	public ReviewService(IValidator<AddOrEditReviewDto> reviewDtoValidator, DomServiceAbstractions.IReviewService reviewService, IMapper mapper)
	{
		_reviewDtoValidator = reviewDtoValidator;
		_reviewService = reviewService;
		_mapper = mapper;
	}

	public async Task<PaginatedResults<ReviewDto>> GetReviews(int bookId, int pageNumber, int pageSize)
	{
		try
		{
			return _mapper.Map<PaginatedResults<ReviewDto>>(await _reviewService.GetReviews(bookId, pageNumber, pageSize));
		}
		catch (DomExceptions.EntityNotFoundException ex)
		{
			throw new AppExceptions.EntityNotFoundException(ex.Message, HttpStatusCode.NotFound, ex);
		}
	}

	public async Task<ReviewDto> GetReview(int reviewId)
	{
		try
		{
			return _mapper.Map<ReviewDto>(await _reviewService.GetReview(reviewId));
		}
		catch (DomExceptions.EntityNotFoundException ex)
		{
			throw new AppExceptions.EntityNotFoundException(ex.Message, HttpStatusCode.NotFound, ex);
		}
	}

	public async Task<(ValidationResult ValidationResult, int? ReviewId)> AddReview(AddOrEditReviewDto review)
	{
		var validationResult = _reviewDtoValidator.Validate(review);
		return (validationResult, validationResult.IsValid ?
			await _reviewService.AddReview(_mapper.Map<AddOrEditReview>(review)) : null);
	}

	public async Task<ValidationResult> EditReview(AddOrEditReviewDto review)
	{
		var validationResult = _reviewDtoValidator.Validate(review);
		if (validationResult.IsValid)
		{
			await _reviewService.EditReview(_mapper.Map<AddOrEditReview>(review));
		}

		return validationResult;
	}

	public async Task DeleteReview(int reviewId)
	{
		try
		{
			await _reviewService.DeleteReview(reviewId);
		}
		catch (DomExceptions.EntityNotFoundException ex)
		{
			throw new AppExceptions.EntityNotFoundException(ex.Message, HttpStatusCode.NotFound);
		}
	}
}
