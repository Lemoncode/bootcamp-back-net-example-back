using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Crosscutting.MappingProfiles;

public class PaginatedResultsConverter<TSource, TDestination> : ITypeConverter<PaginatedResults<TSource>, PaginatedResults<TDestination>>
{
	private readonly IMapper _mapper;

	public PaginatedResultsConverter(IMapper mapper)
	{
		_mapper = mapper;
	}

	public PaginatedResults<TDestination> Convert(PaginatedResults<TSource> source, PaginatedResults<TDestination> destination, ResolutionContext context)
	{
		var mappedData = _mapper.Map<IEnumerable<TDestination>>(source.Results);
		return new PaginatedResults<TDestination>(mappedData, source.PaginationInfo.CurrentPage, source.PaginationInfo.ResultsPerPage, source.PaginationInfo.TotalRows);
	}
}