using FluentValidation;

using Lemoncode.LibraryExample.Api.Extensions;
using Lemoncode.LibraryExample.Api.Middlewares;
using Lemoncode.LibraryExample.Application.Validators.Authors;
using Lemoncode.LibraryExample.DataAccess.Context;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextFactory<LibraryDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddMappings()
	.AddConfigurations(builder.Configuration)
	.AddInfraServices()
	.AddAppServices()
	.AddApiServices()
	.AddJwtAuthentication(builder.Configuration)
	.AddUtilities()
	.AddValidatorsFromAssemblyContaining<AuthorValidator>()
	.AddControllers();

// cors
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		policyBuilder =>
		{
			policyBuilder.AllowAnyOrigin()
				   .AllowAnyMethod()
				   .AllowAnyHeader();
		});
});

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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<SlidingExpirationMiddleware>();
app.MapControllers();

app.Run();
