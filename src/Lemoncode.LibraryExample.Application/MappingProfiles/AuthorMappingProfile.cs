using AutoMapper;
using Lemoncode.LibraryExample.Application.Dtos.Authors;
using Lemoncode.LibraryExample.Domain.Entities.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.MappingProfiles;

public class AuthorMappingProfile : Profile
{

	public AuthorMappingProfile()
	{
		CreateMap<Author, AuthorDto>().ReverseMap();
		CreateMap<AuthorWithBookCount, AuthorWithBookCountDto>();
	}
}
