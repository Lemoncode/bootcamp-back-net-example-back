using AutoMapper;
using DomEntities = Lemoncode.LibraryExample.Domain.Entities;
using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.DataAccess.AutoMapperProfiles;

public class AuthorMappingProfile : Profile
{

	public AuthorMappingProfile()
	{
		CreateMap<DalEntities.Author, DomEntities.AuthorWithBookCount>()
		.ForMember(m => m.BookCount, opt => opt.MapFrom(s => s.Books.Count()));
	}
}
