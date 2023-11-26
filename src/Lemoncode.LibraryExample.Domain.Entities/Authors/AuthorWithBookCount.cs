using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Domain.Entities.Authors;

public class AuthorWithBookCount
{

	public int Id { get; set; }

	public required string FirstName { get; set; }

	public required string LastName { get; set; }

	public int BookCount { get; set; }

}