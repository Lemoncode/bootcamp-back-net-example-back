using System.ComponentModel.DataAnnotations;

namespace Lemoncode.LibraryExample.Application.Dtos.Commands.Authors;

public class AuthorDto
{
	public int Id { get; set; }

	[Required(ErrorMessage = "The name is required.")]
	[StringLength(100, ErrorMessage = "The name must contains 100 characters maximum.")]
	public required string FirstName { get; set; }

	[Required(ErrorMessage = "The last name is required.")]
	[StringLength(100, ErrorMessage = "The last name must contains 100 characters maximum.")]
	public required string LastName { get; set; }
}
