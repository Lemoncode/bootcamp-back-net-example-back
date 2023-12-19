using Lemoncode.LibraryExample.Application.Exceptions;
using Lemoncode.LibraryExample.Domain.Exceptions;

using Microsoft.AspNetCore.Mvc;

namespace Lemoncode.LibraryExample.Api.Extensions;

public static class ControllerExtensions
{
	public static ObjectResult Problem(this ControllerBase controller, HttpExceptionBase exception)
	{
		return controller.Problem(title: "Entity not found.", detail: exception.Message, statusCode: (int)exception.HttpStatusCode);
	}
}
