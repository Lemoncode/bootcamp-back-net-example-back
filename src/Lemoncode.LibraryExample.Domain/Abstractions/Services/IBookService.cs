﻿using Lemoncode.LibraryExample.Domain.Entities;

namespace Lemoncode.LibraryExample.Domain.Abstractions.Services;

public interface IBookService
{
	Task<IEnumerable<Book>> GetNovelties(int limit);

	Task<IEnumerable<Book>> Search(string text);

	Task<IEnumerable<Book>> GetTopRatedBooks();

	Task<IEnumerable<Book>> GetMostDownloadedBooksAsync();

	Task<int> AddBook(AddOrEditBook book);

	Task EditBook(int bookId, AddOrEditBook book);

	Task DeleteBook(int bookId);
}