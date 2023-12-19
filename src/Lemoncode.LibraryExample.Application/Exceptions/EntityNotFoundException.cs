using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Exceptions;

public class EntityNotFoundException : HttpExceptionBase
{
	public EntityNotFoundException()
	{
	}

	public EntityNotFoundException(string message) : base(message)
	{
	}

	public EntityNotFoundException(string message, HttpStatusCode statusCode) : base(message, statusCode)
	{
	}

	public EntityNotFoundException(string message, Exception inner) : base(message, inner)
	{
	}

	public EntityNotFoundException(string message, HttpStatusCode statusCode, Exception inner) : base(message, statusCode, inner)
	{
	}
}
