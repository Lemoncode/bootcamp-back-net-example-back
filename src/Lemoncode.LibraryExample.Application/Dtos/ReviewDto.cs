using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Dtos;

public class ReviewDto
{
	public required string Id { get; set; }

	public required string Reviewer { get; set; }

	public required string Title { get; set; }

	public required string Text { get; set; }
}