﻿using Lemoncode.LibraryExample.DataAccess.Context;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;

namespace Lemoncode.LibraryExample.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly LibraryDbContext _context;

	public UnitOfWork(LibraryDbContext context)
	{
		_context = context;
	}

	public Task CommitAsync() =>
		_context.SaveChangesAsync();

	public void RollbackAsync()
	{
	}
}
