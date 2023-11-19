using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Dtos;

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
