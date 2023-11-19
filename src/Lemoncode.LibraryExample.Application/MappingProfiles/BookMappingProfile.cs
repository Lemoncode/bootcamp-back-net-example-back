using AutoMapper;

using Lemoncode.LibraryExample.Application.Dtos;
using Lemoncode.LibraryExample.Domain.Entities.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.MappingProfiles;

public class BookMappingProfile : Profile
{

	public BookMappingProfile()
	{
		CreateMap<Book, BookDto>();
		CreateMap<BookDto, Book>();
		CreateMap<AddOrEditBook, AddOrEditBookDto>();
		CreateMap<AddOrEditBookDto, AddOrEditBook>();
	}
}
