using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.FileStorage.Config;

public record class BookImageRepositoryConfig
{
	public static readonly string ConfigSection = "Repositories:BookImageRepository";
	
	public required string ImageStoragePath { get; set; }
}
