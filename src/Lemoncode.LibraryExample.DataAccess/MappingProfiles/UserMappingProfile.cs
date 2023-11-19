using AutoMapper;

using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;
using DomEntities = Lemoncode.LibraryExample.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.DataAccess.MappingProfiles;

public class UserMappingProfile : Profile
{

	public UserMappingProfile()
	{
		CreateMap<DalEntities.User, DomEntities.User>();
	}
}
