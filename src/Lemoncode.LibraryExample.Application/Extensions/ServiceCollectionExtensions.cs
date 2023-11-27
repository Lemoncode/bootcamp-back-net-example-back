using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.DataAccess.Repositories;
using DomServices = Lemoncode.LibraryExample.Domain.Services;
using DomServiceAbstractions = Lemoncode.LibraryExample.Domain.Abstractions.Services;
using AppServices = Lemoncode.LibraryExample.Application.Services;
using appServiceAbstractions = Lemoncode.LibraryExample.Application.Abstractions.Services;
using Lemoncode.LibraryExample.DataAccess.Repositories.Helpers;
using Microsoft.Extensions.Configuration;
using Lemoncode.LibraryExample.Application.Config.Validators;
using Lemoncode.LibraryExample.Application.Validators.Books;
using Lemoncode.LibraryExample.FileStorage;
using Lemoncode.LibraryExample.FileStorage.Config;

namespace Lemoncode.LibraryExample.Application.Extensions;

public static class ServiceCollectionExtensions
{

	public static IServiceCollection AddMappings(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddAutoMapper(
			typeof(Lemoncode.LibraryExample.Application.MappingProfiles.AuthorMappingProfile).Assembly,
			typeof(Lemoncode.LibraryExample.DataAccess.MappingProfiles.AuthorMappingProfile).Assembly,
			typeof(Lemoncode.LibraryExample.Crosscutting.MappingProfiles.PaginatedResultsMappingProfile).Assembly);

		return serviceCollection;
	}

	public static IServiceCollection AddUtilities(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IPaginationHelper, PaginationHelper>();

		return serviceCollection;
	}

	public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
		serviceCollection.AddScoped<IAuthorRepository, AuthorRepository>();
		serviceCollection.AddScoped<IBookRepository, BookRepository>();
		serviceCollection.AddScoped<IBookImageRepository, BookImageRepository>();

		return serviceCollection;
	}

	public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<DomServiceAbstractions.IAuthorService, DomServices.AuthorService>();
		serviceCollection.AddScoped<DomServiceAbstractions.IBookService, DomServices.BookService>();

		return serviceCollection;
	}
	public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		serviceCollection.Configure<BookImageUploadDtoValidatorConfig>(configuration.GetSection(BookImageUploadDtoValidatorConfig.ConfigSection));
		serviceCollection.Configure<BookImageRepositoryConfig>(configuration.GetSection(BookImageRepositoryConfig.ConfigSection));
		return serviceCollection;
	}

	public static IServiceCollection AddAppServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<appServiceAbstractions.IAuthorService, AppServices.AuthorService>();
		serviceCollection.AddScoped<appServiceAbstractions.IBookService, AppServices.BookService>();

		return serviceCollection;
	}
}
