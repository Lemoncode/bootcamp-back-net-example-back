using AutoMapper;
using Lemoncode.LibraryExample.Domain.Entities.Reviews;
using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;
using DomEntities = Lemoncode.LibraryExample.Domain.Entities;

namespace Lemoncode.LibraryExample.DataAccess.MappingProfiles;

public class ReviewMappingProfile : Profile
{

	public ReviewMappingProfile()
	{
		CreateMap<AddOrEditReview, DalEntities.Review>()
			.ForMember(m => m.CreationDate, opt => opt.MapFrom(src => DateTime.UtcNow));
		CreateMap<DalEntities.Review, DomEntities.Reviews.Review>();
	}
}
