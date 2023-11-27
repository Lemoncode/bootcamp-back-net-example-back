using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Domain.Entities.Books;

public class AddOrEditBook
{
	public enum OperationType
	{
		Add,
		Edit
	}

	public required OperationType Operation { get; set; }

	public required string Title { get; set; }

	public required int[] AuthorIds { get; set; }

	public required string Description { get; set; }

	public required string TempImageFileName { get; set; }

	public required string ImageAltText { get; set; }

	public required DateTime Created { get; set; }

}
