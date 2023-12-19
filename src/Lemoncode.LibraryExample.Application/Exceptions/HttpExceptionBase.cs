using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Exceptions;


[Serializable]
public class HttpExceptionBase : Exception
{

	public HttpStatusCode HttpStatusCode { get; set; }

	public HttpExceptionBase() { }
	
	public HttpExceptionBase(string message) : base(message) { }
	
	public HttpExceptionBase(string message, HttpStatusCode statusCode) : base(message)
	{
		this.HttpStatusCode = statusCode;
	}
	
	public HttpExceptionBase(string message, Exception inner) : base(message, inner) { }

	public HttpExceptionBase(string message, HttpStatusCode statusCode, Exception inner) : base(message, inner)
	{
		this.HttpStatusCode = statusCode;
	}
}