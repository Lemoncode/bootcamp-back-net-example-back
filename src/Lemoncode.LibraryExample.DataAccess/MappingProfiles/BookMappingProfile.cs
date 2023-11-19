using AutoMapper;

using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;
using DomEntities = Lemoncode.LibraryExample.Domain.Entities.Book;

namespace Lemoncode.LibraryExample.DataAccess.MappingProfiles;

public class BookMappingProfile : Profile
{
	public BookMappingProfile()
	{
		CreateMap<DomEntities.AddOrEditBook, DalEntities.Book>();
		CreateMap<DalEntities.Book, DomEntities.Book>();
	}
}
