using AutoMapper;
using Lemoncode.LibraryExample.Application.Dtos.Reviews;
using Lemoncode.LibraryExample.Domain.Entities.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.MappingProfiles;

public class ReviewMappingProfile : Profile
{

	public ReviewMappingProfile()
	{
		CreateMap<Review, ReviewDto>().ReverseMap();
		CreateMap<AddOrEditReviewDto, AddOrEditReview>();
	}
}
