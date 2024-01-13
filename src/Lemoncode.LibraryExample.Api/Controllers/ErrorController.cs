using Microsoft.AspNetCore.Mvc;

namespace Lemoncode.LibraryExample.Api.Controllers
{
	[ApiController]
	[Route("Error")]
	public class ErrorController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return Problem();
		}
	}
}
