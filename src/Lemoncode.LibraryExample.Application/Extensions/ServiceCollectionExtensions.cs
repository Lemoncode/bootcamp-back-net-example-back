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

namespace Lemoncode.LibraryExample.Application.Extensions;

public static class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterMapping(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddAutoMapper(typeof(MappingProfiles.AuthorMappingProfile).Assembly, typeof(DataAccess.MappingProfiles.AuthorMappingProfile).Assembly);
		return serviceCollection;
	}

	public static IServiceCollection RegisterUtilities(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IPaginationHelper, PaginationHelper>();

		return serviceCollection;
	}

	public static IServiceCollection RegisterRepositories(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
		serviceCollection.AddScoped<IAuthorRepository, AuthorRepository>();
		serviceCollection.AddScoped<IBookRepository, BookRepository>();

		return serviceCollection;
	}

	public static IServiceCollection RegisterDomainServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<DomServiceAbstractions.IAuthorService, DomServices.AuthorService>();
		serviceCollection.AddScoped<DomServiceAbstractions.IBookService, DomServices.BookService>();

		return serviceCollection;
	}

	public static IServiceCollection RegisterAppServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<appServiceAbstractions.IAuthorService, AppServices.AuthorService>();
		serviceCollection.AddScoped<appServiceAbstractions.IBookService, AppServices.BookService>();

		return serviceCollection;
	}
}
