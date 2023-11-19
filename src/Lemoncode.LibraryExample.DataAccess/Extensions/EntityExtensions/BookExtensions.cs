﻿using DomEntities=Lemoncode.LibraryExample.Domain.Entities.Book;
using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.DataAccess.Extensions.EntityExtensions;

internal static class AddOrEditBookExtensions
{

	internal static void UpdateDalBook(this DalEntities.Book existingBook, DomEntities.AddOrEditBook updatedBook)
	{
		existingBook.Title = updatedBook.Title;
		existingBook.Description = updatedBook.Description;
		existingBook.ImageAltText = updatedBook.ImageAltText;
	}
}
