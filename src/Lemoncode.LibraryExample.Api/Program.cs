using Lemoncode.LibraryExample.Api.Extensions;
using Lemoncode.LibraryExample.Application.Validators.Books;
using Lemoncode.LibraryExample.DataAccess.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Lemoncode.LibraryExample.Domain.Entities.Validators.Books;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Lemoncode.LibraryExample.Api.Config;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextFactory<LibraryDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddMappings()
	.AddConfigurations(builder.Configuration)
	.AddUtilities()
	.AddInfraServices()
	.AddDomainServices()
	.AddAppServices()
	.AddApiServices()
	.AddJwtAuthentication(builder.Configuration)
	.AddValidatorsFromAssemblyContaining<BookImageUploadDtoValidator>()
	.AddValidatorsFromAssemblyContaining<BookValidator>()
	.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
	.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseExceptionHandler("/error");
	app.UseHsts();
}

app.UseHttpsRedirection()
	.UseAuthentication()
	.UseAuthorization();

app.MapControllers();

app.Run();
