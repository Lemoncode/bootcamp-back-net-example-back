using AutoMapper;
using Lemoncode.LibraryExample.Domain.Entities.Books;
using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;
using DomEntities = Lemoncode.LibraryExample.Domain.Entities;

namespace Lemoncode.LibraryExample.DataAccess.MappingProfiles;

public class BookMappingProfile : Profile
{
	public BookMappingProfile()
	{
		CreateMap<AddOrEditBook, DalEntities.Book>();
		CreateMap<DalEntities.Book, DomEntities.Book>().ReverseMap();
	}
}
