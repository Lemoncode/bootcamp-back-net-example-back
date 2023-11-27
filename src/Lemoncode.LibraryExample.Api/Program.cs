using Lemoncode.LibraryExample.Application.Extensions;
using Lemoncode.LibraryExample.Application.Validators.Books;
using Lemoncode.LibraryExample.DataAccess.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Lemoncode.LibraryExample.Domain.Entities.Validators.Books;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextFactory<LibraryDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddMappings()
	.AddUtilities()
	.AddRepositories()
	.AddDomainServices()
	.AddAppServices()
	.AddConfigurations(builder.Configuration)
	.AddValidatorsFromAssemblyContaining<BookImageUploadDtoValidator>()
	.AddValidatorsFromAssemblyContaining<AddEditBookValidator>()
	.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
