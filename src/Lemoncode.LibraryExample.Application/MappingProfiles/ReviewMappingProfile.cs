﻿using AutoMapper;

using Lemoncode.LibraryExample.Application.Dtos;
using Lemoncode.LibraryExample.Domain.Entities;

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
		CreateMap<Review, ReviewDto>();
		CreateMap<ReviewDto, Review>();
	}
}