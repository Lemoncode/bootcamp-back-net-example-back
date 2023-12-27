using Lemoncode.LibraryExample.Application.Exceptions;

using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace Lemoncode.LibraryExample.Api.Extensions;

public static class ControllerExtensions
{
	private static readonly Dictionary<Type, HttpStatusCode> ExceptionToHttpCodeMap = new()
	{
		[typeof(EntityNotFoundException)] = HttpStatusCode.NotFound	
	};
	
	public static ObjectResult Problem(this ControllerBase controller, Exception exception)
	{
		HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
		var exceptionType = typeof(Exception);
		if (ExceptionToHttpCodeMap.ContainsKey(exceptionType))
		{
			statusCode = ExceptionToHttpCodeMap[exceptionType];
		}

		return controller.Problem(title: statusCode.ToString(), detail: exception.Message, statusCode: (int)statusCode);
	}
}
