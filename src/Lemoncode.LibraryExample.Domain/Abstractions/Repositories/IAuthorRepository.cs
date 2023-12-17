using Lemoncode.LibraryExample.Crosscutting;
using Lemoncode.LibraryExample.Domain.Abstractions.Entities;
using Lemoncode.LibraryExample.Domain.Entities;
using Lemoncode.LibraryExample.Domain.Entities.Authors;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Domain.Abstractions.Repositories;

public interface IAuthorRepository
{

	Task<PaginatedResults<AuthorWithBookCount>> GetAuthors(int pageNumber, int pageSize);
	
	Task<AuthorWithBookCount> GetAuthor(int authorId);

	Task<Author> GetAuthorById(int authorId);

	Task<bool> AuthorExists(int authorId);

	Task<bool> AuthorsExist(int[] authorIds);

	Task<IIdentifiable> AddAuthor(Author author);

	Task EditAuthor(Author author);

	Task DeleteAuthor(int authorId);
}
