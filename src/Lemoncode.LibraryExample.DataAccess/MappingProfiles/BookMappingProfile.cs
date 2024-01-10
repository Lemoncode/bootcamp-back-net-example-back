using AutoMapper;
using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;
using DomEntities = Lemoncode.LibraryExample.Domain.Entities.Books;

namespace Lemoncode.LibraryExample.DataAccess.MappingProfiles;

public class BookMappingProfile : Profile
{
	public BookMappingProfile()
	{
		CreateMap<DalEntities.Book, DomEntities.Book>()
			.ForMember(m => m.Authors, opt => opt.Ignore())
			.ConstructUsing(s =>
			new DomEntities.Book(
				s.Id,
				s.Title,
				new DomEntities.BookDescription(s.Description),
				new DomEntities.BookImage(s.ImageFileName, s.ImageAltText),
				s.Authors.Select(a => a.Id).ToList()));

		CreateMap<DomEntities.Book, DalEntities.Book>()
			.ForMember(m => m.Description, opt => opt.MapFrom(s => s.Description.Description))
			.ForMember(m => m.ImageAltText, opt => opt.MapFrom(s => s.Image.AltText))
			.ForMember(m => m.ImageFileName, opt => opt.MapFrom(s => s.Image.FileName))
			// Ignoramos autores, puesto que los ajustamos manualmente en el repositorio
			.ForMember(m => m.Authors, opt => opt.Ignore());
	}
}
