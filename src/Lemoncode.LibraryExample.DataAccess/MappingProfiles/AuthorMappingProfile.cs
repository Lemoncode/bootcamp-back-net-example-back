using AutoMapper;

using Lemoncode.LibraryExample.Domain.Entities.Authors;

using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;

namespace Lemoncode.LibraryExample.DataAccess.MappingProfiles;

public class AuthorMappingProfile : Profile
{

	public AuthorMappingProfile()
	{
		CreateMap<DalEntities.Author, Author>();
		CreateMap<Author, DalEntities.Author>();
		CreateMap<DalEntities.Author, AuthorWithBookCount>()
		.ForMember(m => m.BookCount, opt => opt.MapFrom(s => s.Books.Count()));
	}
}
