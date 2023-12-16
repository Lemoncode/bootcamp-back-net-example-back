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
		CreateMap<Book, BookDto>()
			.ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => $"/api/books/{s.Id}/image"));
		CreateMap<AddOrEditBook, AddOrEditBookDto>().ReverseMap();
		CreateMap<IFormFile, BookImageUploadDto>()
			.ForMember(d => d.BinaryData, opt => opt.MapFrom(s =>  s.OpenReadStream()));
		CreateMap<BookImageUploadDto, BookImageUpload>();
	}
}
