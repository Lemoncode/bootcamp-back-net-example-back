using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Abstractions.Services;
using Lemoncode.LibraryExample.Domain.Entities;

namespace Lemoncode.LibraryExample.Domain.Services
{
	public class BookService : IBookService
	{

		private readonly IBookRepository _repository;

		private readonly IUnitOfWork _unitOfWork;

		public BookService(IBookRepository repository, IUnitOfWork unitOfWork)
		{
			_repository = repository;
			_unitOfWork = unitOfWork;
		}

		public Task AddBook(Book book)
		{
			throw new NotImplementedException();
		}

		public void DeleteBook(Book book)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Book>> GetMostDownloadedBooksAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Book>> GetNoveltiesAsync(int limit)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Book>> GetTopRatedBooksAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Book>> SearchByTitleAsync(string text)
		{
			throw new NotImplementedException();
		}
	}
}
