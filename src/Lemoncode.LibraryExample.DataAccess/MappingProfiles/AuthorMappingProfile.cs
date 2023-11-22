using AutoMapper;
using DomEntities = Lemoncode.LibraryExample.Domain.Entities;
using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lemoncode.LibraryExample.Domain.Entities;

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
