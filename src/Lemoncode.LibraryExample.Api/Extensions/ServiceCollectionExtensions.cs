using Lemoncode.LibraryExample.Api.Config;
using Lemoncode.LibraryExample.Api.Services;
using Lemoncode.LibraryExample.Application.Config.Validators;
using Lemoncode.LibraryExample.AuthPlatform.Abstractions;
using Lemoncode.LibraryExample.AuthPlatform.Abstractions.IdentityProviders;
using Lemoncode.LibraryExample.AuthPlatform.Config;
using Lemoncode.LibraryExample.AuthPlatform.IdentityProviders;
using Lemoncode.LibraryExample.DataAccess.Repositories;
using Lemoncode.LibraryExample.DataAccess.Repositories.Helpers;
using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.FileStorage;
using Lemoncode.LibraryExample.FileStorage.Config;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using MimeDetective;

using System.Text;

using appServiceAbstractions = Lemoncode.LibraryExample.Application.Abstractions.Services;
using AppServices = Lemoncode.LibraryExample.Application.Services;
using DomServiceAbstractions = Lemoncode.LibraryExample.Domain.Abstractions.Services;
using DomServices = Lemoncode.LibraryExample.Domain.Services;

namespace Lemoncode.LibraryExample.Api.Extensions;

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
		serviceCollection.AddSingleton<ContentInspector>((serviceProvider) =>
			new ContentInspectorBuilder()
			{
				Definitions = MimeDetective.Definitions.Default.All()
			}.Build()
		);

		return serviceCollection;
	}

	public static IServiceCollection AddInfraServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
		serviceCollection.AddScoped<IAuthorRepository, AuthorRepository>();
		serviceCollection.AddScoped<IBookRepository, BookRepository>();
		serviceCollection.AddScoped<IBookImageRepository, BookImageRepository>();
		serviceCollection.AddScoped<IReviewRepository, ReviewRepository>();
		serviceCollection.AddScoped<IGoogleOauthService, GoogleOauthService>();

		return serviceCollection;
	}

	public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<DomServiceAbstractions.IAuthorService, DomServices.AuthorService>();
		serviceCollection.AddScoped<DomServiceAbstractions.IBookService, DomServices.BookService>();
		serviceCollection.AddScoped<DomServiceAbstractions.IReviewService, DomServices.ReviewService>();

		return serviceCollection;
	}

	public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		serviceCollection.Configure<BookImageUploadDtoValidatorConfig>(configuration.GetSection(BookImageUploadDtoValidatorConfig.ConfigSection));
		serviceCollection.Configure<BookImageRepositoryConfig>(configuration.GetSection(BookImageRepositoryConfig.ConfigSection));
		serviceCollection.Configure<JwtConfig>(configuration.GetSection(JwtConfig.ConfigSection));
		serviceCollection.Configure<GoogleConfig>(configuration.GetSection(GoogleConfig.ConfigSection));
		serviceCollection.Configure<FrontendConfig>(configuration.GetSection(FrontendConfig.ConfigSection));

		return serviceCollection;
	}
	public static IServiceCollection AddApiServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IJWTService, JwtService>();
		return serviceCollection;
	}

	public static IServiceCollection AddAppServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<appServiceAbstractions.IAuthorService, AppServices.AuthorService>();
		serviceCollection.AddScoped<appServiceAbstractions.IBookService, AppServices.BookService>();
		serviceCollection.AddScoped<appServiceAbstractions.IReviewService, AppServices.ReviewService>();

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
