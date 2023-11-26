using AutoMapper;
using Lemoncode.LibraryExample.Application.Dtos.Books;
using Lemoncode.LibraryExample.Domain.Entities;
using Lemoncode.LibraryExample.Domain.Entities.Books;

using Microsoft.AspNetCore.Http;

namespace Lemoncode.LibraryExample.Application.MappingProfiles;

public class BookMappingProfile : Profile
{

	public BookMappingProfile()
	{
		CreateMap<Book, BookDto>().ReverseMap();
		CreateMap<AddOrEditBook, AddOrEditBookDto>().ReverseMap();
		CreateMap<IFormFile, BookImageUploadDto>()
			.ForMember(d => d.BinaryData, opt => opt.MapFrom(s =>  s.OpenReadStream()));
	}
}
