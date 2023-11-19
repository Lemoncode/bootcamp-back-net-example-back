using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Crosscutting.MappingProfiles;

public class PaginatedResultsMappingProfile : Profile
{

	public PaginatedResultsMappingProfile()
	{
		CreateMap(typeof(PaginatedResults<>), typeof(PaginatedResults<>))
		.ConvertUsing(typeof(PaginatedResultsConverter<,>));
	}
}
