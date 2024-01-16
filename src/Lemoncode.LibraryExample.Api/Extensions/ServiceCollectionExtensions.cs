using Lemoncode.LibraryExample.Api.Config;
using Lemoncode.LibraryExample.Api.Services;
using Lemoncode.LibraryExample.Application.Config;
using Lemoncode.LibraryExample.Application.Config.Validators;
using Lemoncode.LibraryExample.AuthPlatform.Abstractions;
using Lemoncode.LibraryExample.AuthPlatform.Abstractions.IdentityProviders;
using Lemoncode.LibraryExample.AuthPlatform.Config;
using Lemoncode.LibraryExample.AuthPlatform.IdentityProviders;
using Lemoncode.LibraryExample.DataAccess.Repositories;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.FileStorage;
using Lemoncode.LibraryExample.FileStorage.Config;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;

using MimeDetective;

using System.Text;

using appQueryServiceAbstractions = Lemoncode.LibraryExample.Application.Abstractions.Queries;
using AppQueryServices = Lemoncode.LibraryExample.Application.Queries;
using appServiceAbstractions = Lemoncode.LibraryExample.Application.Abstractions.Services;
using AppServices = Lemoncode.LibraryExample.Application.Services;

namespace Lemoncode.LibraryExample.Api.Extensions;

public static class ServiceCollectionExtensions
{

	public static IServiceCollection AddMappings(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddAutoMapper(
			typeof(Lemoncode.LibraryExample.Api.MappingProfiles.BookMappingProfile).Assembly,
			typeof(Lemoncode.LibraryExample.DataAccess.MappingProfiles.AuthorMappingProfile).Assembly);

		return serviceCollection;
	}


	public static IServiceCollection AddInfraServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
		serviceCollection.AddScoped<IAuthorRepository, AuthorRepository>();
		serviceCollection.AddScoped<IBookRepository, BookRepository>();
		serviceCollection.AddScoped<IFileRepository, FileRepository>();
		serviceCollection.AddScoped<IGoogleOauthService, GoogleOauthService>();

		return serviceCollection;
	}

	public static IServiceCollection AddUtilities(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<ContentInspector>((serviceProvider) =>
			new ContentInspectorBuilder()
			{
				Definitions = MimeDetective.Definitions.Default.All()
			}.Build()
		);

		return serviceCollection;
	}

	public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		serviceCollection.Configure<BookImageUploadDtoValidatorConfig>(configuration.GetSection(BookImageUploadDtoValidatorConfig.ConfigSection));
		serviceCollection.Configure<FileStorageRepositoryConfig>(configuration.GetSection(FileStorageRepositoryConfig.ConfigSection));
		serviceCollection.Configure<JwtConfig>(configuration.GetSection(JwtConfig.ConfigSection));
		serviceCollection.Configure<GoogleConfig>(configuration.GetSection(GoogleConfig.ConfigSection));
		serviceCollection.Configure<FrontendConfig>(configuration.GetSection(FrontendConfig.ConfigSection));
		serviceCollection.Configure<DapperConfig>(configuration.GetSection(DapperConfig.ConfigurationSection));

		return serviceCollection;
	}


	public static IServiceCollection AddAppServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<appServiceAbstractions.IBookService, AppServices.BookService>();
		serviceCollection.AddScoped<appServiceAbstractions.IAuthorService, AppServices.AuthorService>();
		serviceCollection.AddScoped<appQueryServiceAbstractions.IAuthorQueriesService, AppQueryServices.AuthorQueriesService>();
		serviceCollection.AddScoped<appQueryServiceAbstractions.IBookQueriesService, AppQueryServices.BookQueriesService>();

		return serviceCollection;
	}

	public static IServiceCollection AddApiServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IJWTService, JwtService>();
		return serviceCollection;
	}

	public static IServiceCollection AddJwtAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		var jwtConfig = configuration.GetSection(JwtConfig.ConfigSection).Get<JwtConfig>()!;
		serviceCollection.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.Events = new JwtBearerEvents
			{
				OnMessageReceived = (context) =>
				{
					var cookie = context.Request.Cookies["AuthToken"];
					if (cookie != null)
					{
						context.Token = cookie;
					}

					return Task.CompletedTask;
				}
			};
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = jwtConfig.Issuer,
				ValidAudience = jwtConfig.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SigningKey))
			};
		});
		return serviceCollection;
	}
}
