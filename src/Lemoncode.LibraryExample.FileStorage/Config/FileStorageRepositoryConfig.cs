using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.FileStorage.Config;

public record class FileStorageRepositoryConfig
{
	public static readonly string ConfigSection = "Repositories:FileStorageRepository";
	
	public required string FileStoragePath { get; set; }
}
