using AutoMapper;

using Lemoncode.LibraryExample.Domain.Entities.Books;

using DalEntities = Lemoncode.LibraryExample.DataAccess.Entities;
using DomEntities = Lemoncode.LibraryExample.Domain.Entities;

namespace Lemoncode.LibraryExample.DataAccess.MappingProfiles;

public class ReviewMappingProfile : Profile
{

	public ReviewMappingProfile()
	{
		CreateMap<DalEntities.Review, Review>()
			.ConstructUsing(s =>
			new Review(
				s.Id,
				s.BookId,
				s.Reviewer,
				new ReviewText(s.ReviewText),
				s.Stars));
		CreateMap<DomEntities.Books.Review, DalEntities.Review>()
			.ForMember(m => m.ReviewText, opt => opt.MapFrom(s => s.Text.Text));
	}
}
