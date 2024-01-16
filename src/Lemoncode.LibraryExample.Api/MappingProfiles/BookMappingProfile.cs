using AutoMapper;

using Lemoncode.LibraryExample.Api.Models.Queries;
using Lemoncode.LibraryExample.Application.Dtos.Queries.Books;

namespace Lemoncode.LibraryExample.Api.MappingProfiles;

public class BookMappingProfile : Profile
{

	public BookMappingProfile()
	{
		CreateMap<BookDto, Book>()
			.ForMember(m => m.ImageUrl, opt => opt.MapFrom(src => $"/api/books/{src.Id}/image"));

	}
}
