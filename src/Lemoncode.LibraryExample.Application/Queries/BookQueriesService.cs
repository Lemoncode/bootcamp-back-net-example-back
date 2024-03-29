﻿using Dapper;

using Lemoncode.LibraryExample.Application.Abstractions.Queries;
using Lemoncode.LibraryExample.Application.Config;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Authors;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;
using Lemoncode.LibraryExample.Application.Exceptions;
using Lemoncode.LibraryExample.Application.Queries.Pagination;
using Lemoncode.LibraryExample.FileStorage;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

using MimeDetective;

namespace Lemoncode.LibraryExample.Application.Queries;

public class BookQueriesService : QueryServiceBase, IBookQueriesService
{
	private const string MimeTypeContentStream = "application/octet-stream";

	private readonly ContentInspector _contentInspector;

	private readonly IFileRepository _fileRepository;

	public BookQueriesService(IOptionsSnapshot<DapperConfig> dapperConfig, IFileRepository fileRepository, ContentInspector contentInspector) : base(dapperConfig)
	{
		_fileRepository = fileRepository;
		_contentInspector = contentInspector;
	}

	public async Task<BookImageUploadDto> GetBookImage(int bookId)
	{
		bool notFound = false;
		Stream? image = null;
		var imageFileName = await SqlConnection.QuerySingleOrDefaultAsync<string>("SELECT ImageFileName FROM Books WHERE Id=@id", new { Id = bookId });
		if (imageFileName is null)
		{
			notFound = true;
		}
		else
		{
			image = _fileRepository.GetFile(new Uri(imageFileName));
			if (image is null)
			{
				notFound = true;
			}
		}
		if (notFound)
		{
			throw new EntityNotFoundException($"Unable to find an image associated to the book with Id {bookId}.");
		}

		var mimeResult = _contentInspector.Inspect(image!);
		var contentType = !mimeResult.Any() ? MimeTypeContentStream : mimeResult[0].Definition.File.MimeType;
		image!.Seek(0, SeekOrigin.Begin);

		return new BookImageUploadDto { BinaryData = image!, FileName = imageFileName!, ContentType = contentType! };
	}

	public async Task<BookDto> GetBook(int bookId)
	{
		var book = await SqlConnection.QuerySingleOrDefaultAsync<BookDto>(@"SELECT Id, Title, Description, ImageAltText, Created FROM Books
			where Id=@id",
			new { id = bookId });

		if (book is null)
		{
			throw new EntityNotFoundException($"Unable to find a book with Id {bookId}.");
		}

		var authors = await SqlConnection.QueryAsync<AuthorDto>(@"SELECT a.Id, a.FirstName, a.LastName 
			FROM Authors a inner join AuthorBook b on a.Id = b.AuthorsId where b.BooksId=@id",
			new { id = bookId });
		book.Authors = authors.ToList();
		book.ReviewCount = await SqlConnection.QuerySingleOrDefaultAsync<int>("SELECT COUNT(1) FROM Reviews WHERE BookId=@id",
			new { id = bookId });

		return book;
	}

	public async Task<IList<BookDto>> GetNoveltiesAsync(int limit)
	{
		var sqlQuery = @"SELECT TOP (@N) 
			b.Id, b.Title, b.Description, b.ImageAltText, b.Created, COUNT(r.BookId) as ReviewCount, 
			a.FirstName, a.LastName, a.Id 
			FROM Books b
			INNER JOIN AuthorBook ab ON b.Id = ab.BooksId
			INNER JOIN Authors a ON ab.AuthorsId = a.Id
			LEFT JOIN Reviews r ON b.Id = r.BookId
			GROUP BY b.Id, b.Title, b.Description, b.ImageAltText, b.Created, a.FirstName, a.LastName, a.Id
			ORDER BY b.Created DESC";

		var booksDictionary = new Dictionary<int, BookDto>();

		var books = await SqlConnection.QueryAsync<BookDto, AuthorDto, BookDto>(
			sql: sqlQuery,
			map: (book, author) =>
			{
				BookDto? bookEntry;

				if (!booksDictionary.TryGetValue(book.Id, out bookEntry))
				{
					bookEntry = book;
					bookEntry.Authors = new List<AuthorDto>();
					booksDictionary.Add(bookEntry.Id, bookEntry);
				}

				bookEntry.Authors.Add(author);
				return bookEntry;
			},
			splitOn: "FirstName",
			param: new { N = limit }
		);

		books = books.Distinct();

		return books.ToList();
	}

	public async Task<PaginatedResults<ReviewDto>> GetReviews(int bookId, int page, int pageSize)
	{
		PaginationHelper.CheckPaginationValidity(page, pageSize);
		var totalReviews = await SqlConnection.QuerySingleOrDefaultAsync<int>("SELECT COUNT(1) FROM Reviews where BookId=@bookId", new { bookId = bookId });
		var result = await PaginationHelper.PaginateAsync(totalReviews, page, pageSize, async (offset, fetch) =>
		{
			return await SqlConnection.QueryAsync<ReviewDto>(
			@"SELECT
				    r.Id,
				r.BookId,
				r.Reviewer,
				r.ReviewText,
				r.Stars,
				r.CreationDate
				FROM
				Reviews r
				where r.BookId=@bookId
				ORDER BY r.CreationDate DESC
				OFFSET @offset ROWS
				FETCH NEXT @fetch ROWS ONLY",
				new { bookId = bookId, offset = offset, fetch = fetch });
		});

		return result;
	}

	public async Task<ReviewDto> GetReview(int bookId, int reviewId)
	{
		var review = await SqlConnection.QuerySingleOrDefaultAsync<ReviewDto>(@"SELECT
				    r.Id,
				r.BookId,
				r.Reviewer,
				r.ReviewText,
				r.Stars,
				r.CreationDate
				FROM
				Reviews r
				where r.BookId=@bookId
				and r.Id = @reviewId",
				new { bookId = bookId, reviewId = reviewId });

		if (review is null)
		{
			throw new EntityNotFoundException($"Unable to find a review with Id {reviewId} in the book with Id {bookId}.");
		}

		return review;
	}
}