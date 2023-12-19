using AutoMapper;
using AutoMapper.QueryableExtensions;

using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.DataAccess.Context;
using Lemoncode.LibraryExample.DataAccess.Repositories.Helpers;
using Lemoncode.LibraryExample.Domain.Abstractions.Entities;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Entities.Authors;
using Lemoncode.LibraryExample.Domain.Entities.Reviews;
using Lemoncode.LibraryExample.Domain.Exceptions;

using Microsoft.EntityFrameworkCore;

using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;

namespace Lemoncode.LibraryExample.DataAccess.Repositories;

public class ReviewRepository : IReviewRepository
{

	private readonly LibraryDbContext _context;

	private readonly IPaginationHelper _paginationHelper;

	private readonly IMapper _mapper;

	public ReviewRepository(LibraryDbContext context, IPaginationHelper paginationHelper, IMapper mapper)
	{
		_context = context;
		_paginationHelper = paginationHelper;
		_mapper = mapper;
	}

	public Task<PaginatedResults<Review>> GetReviews(int bookId, int pageNumber, int pageSize)
	{
		return _paginationHelper.PaginateIQueryableAsync(_context.Reviews
			.Where(r => r.Book.Id == bookId)
			.OrderByDescending(r => r.CreationDate)
			.ProjectTo<Review>(_mapper.ConfigurationProvider),
			pageNumber, pageSize);
	}

	public async Task<Review> GetReviewById(int reviewId)
	{
		var review = await _context.Reviews.FindAsync(reviewId);
		return _mapper.Map<Review>(review);
	}

	public Task<bool> ReviewExists(int reviewId)
	{
		return _context.Reviews.AnyAsync(r => r.Id == reviewId);
	}

	public Task<IIdentifiable> AddReview(AddOrEditReview review)
	{
		var dalReview = _mapper.Map<DalEntities.Review>(review);
		_context.Reviews.Add(dalReview);
		return Task.FromResult((IIdentifiable)dalReview);
	}

	public async Task EditReview(AddOrEditReview review)
	{
		var reviewFromDb = await _context.Reviews.FindAsync(review.Id);
		if (reviewFromDb is null)
		{
			throw new EntityNotFoundException($"The review with ID {review.Id} was not found.");
		}
		
		_mapper.Map(review, reviewFromDb);
	}

	public async Task DeleteReview(int reviewId)
	{
		var reviewFromDb = await _context.Reviews.FindAsync(reviewId);
		if (reviewFromDb is null)
		{
			throw new EntityNotFoundException($"The author with ID {reviewId} was not found.");
		}
		
		_context.Reviews.Remove(reviewFromDb);
	}
}
