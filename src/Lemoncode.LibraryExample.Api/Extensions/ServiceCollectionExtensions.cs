using Lemoncode.LibraryExample.Application.Config;
using Lemoncode.LibraryExample.DataAccess.Repositories;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;

using appServiceAbstractions = Lemoncode.LibraryExample.Application.Abstractions.Services;
using appQueryServiceAbstractions = Lemoncode.LibraryExample.Application.Abstractions.Queries;
using AppServices = Lemoncode.LibraryExample.Application.Services;
using AppQueryServices = Lemoncode.LibraryExample.Application.Queries;
using Lemoncode.LibraryExample.FileStorage.Config;
using Lemoncode.LibraryExample.Application.Config.Validators;
using Lemoncode.LibraryExample.FileStorage;

namespace Lemoncode.LibraryExample.Api.Extensions;

public static class ServiceCollectionExtensions
{

	public static IServiceCollection AddMappings(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddAutoMapper(
			typeof(Lemoncode.LibraryExample.DataAccess.MappingProfiles.AuthorMappingProfile).Assembly);

		return serviceCollection;
	}


	public static IServiceCollection AddInfraServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
		serviceCollection.AddScoped<IAuthorRepository, AuthorRepository>();
		serviceCollection.AddScoped<IBookRepository, BookRepository>();
		serviceCollection.AddScoped<IFileRepository, FileRepository>();

		return serviceCollection;
	}

	public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		serviceCollection.Configure<BookImageUploadDtoValidatorConfig>(configuration.GetSection(BookImageUploadDtoValidatorConfig.ConfigSection));
		serviceCollection.Configure<FileStorageRepositoryConfig>(configuration.GetSection(FileStorageRepositoryConfig.ConfigSection));

		serviceCollection.Configure<DapperConfig>(configuration.GetSection(DapperConfig.ConfigurationSection));
		return serviceCollection;
	}


	public static IServiceCollection AddAppServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<appServiceAbstractions.IBookService, AppServices.BookService>();
		serviceCollection.AddScoped<appServiceAbstractions.IAuthorService, AppServices.AuthorService>();
		serviceCollection.AddScoped<appQueryServiceAbstractions.IAuthorQueriesService, AppQueryServices.AuthorQueriesService>();

		return serviceCollection;
	}

}
