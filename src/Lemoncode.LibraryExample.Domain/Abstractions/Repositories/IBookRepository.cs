using Lemoncode.LibraryExample.Domain.Abstractions.Entities;
using Lemoncode.LibraryExample.Domain.Entities;
using Lemoncode.LibraryExample.Domain.Entities.Books;

namespace Lemoncode.LibraryExample.Domain.Abstractions.Repositories;

public interface IBookRepository
{

	Task<Book> GetBook(int bookId);

	Task<IIdentifiable> AddBook(Book book);

	Task EditBook(Book book);

	Task DeleteBook(int bookId);
	
	Task<bool> BookExists(int bookId);

	Task<Review> GetReview(int reviewId);

	Task<bool> ReviewExists(int reviewId);

	Task<IIdentifiable> AddReview(Review review);

	Task EditReview(Review review);

	Task DeleteReview(int reviewId);
}
