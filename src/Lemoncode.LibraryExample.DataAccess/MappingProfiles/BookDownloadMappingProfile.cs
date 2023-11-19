using AutoMapper;

using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;
using DomEntities = Lemoncode.LibraryExample.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.DataAccess.MappingProfiles;

public class BookDownloadMappingProfile : Profile
{

	public BookDownloadMappingProfile()
	{
		CreateMap<DalEntities.BookDownload, DomEntities.BookDownload>();
	}
}
