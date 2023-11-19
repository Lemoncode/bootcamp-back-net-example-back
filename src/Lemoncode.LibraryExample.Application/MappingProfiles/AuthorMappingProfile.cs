using AutoMapper;

using Lemoncode.LibraryExample.Application.Dtos;
using Lemoncode.LibraryExample.Domain.Entities;
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
		CreateMap<Author, AuthorDto>();
		CreateMap<AuthorDto, Author>();
		CreateMap<AuthorWithBookCount, AuthorWithBookCountDto>();
	}
}
