using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Application.Dtos.Queries.Books;

public class ReviewDto
{

	public int Id { get; set; }

	public int BookId { get; set; }

	public required string Reviewer { get; set; }

	public required string ReviewText { get; set; }

	public DateTime CreationDate { get; set; }

	public ushort Stars { get; set; }
}