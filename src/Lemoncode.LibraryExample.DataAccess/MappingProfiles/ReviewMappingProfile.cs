using AutoMapper;

using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;
using DomEntities = Lemoncode.LibraryExample.Domain.Entities;

namespace Lemoncode.LibraryExample.DataAccess.MappingProfiles;

public class ReviewMappingProfile : Profile
{

	public ReviewMappingProfile()
	{
		CreateMap<DalEntities.Review, DomEntities.Review>();
	}
}
