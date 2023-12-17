using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lemoncode.LibraryExample.Api.Controllers
{
	[ApiController]
	[Route("Error")]public class ErrorController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return Problem();
		}
	}
}
