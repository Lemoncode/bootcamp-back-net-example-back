using AutoMapper;

using Lemoncode.LibraryExample.Domain.Entities.Authors;

using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;

namespace Lemoncode.LibraryExample.DataAccess.MappingProfiles;

public class AuthorMappingProfile : Profile
{

	public AuthorMappingProfile()
	{
		CreateMap<DalEntities.Author, Author>()
			.ConstructUsing(s => new Author(s.Id, s.FirstName, s.LastName));
		CreateMap<Author, DalEntities.Author>();
	}
}
