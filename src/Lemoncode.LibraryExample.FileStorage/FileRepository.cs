using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Entities.Books;
using Lemoncode.LibraryExample.FileStorage.Config;

using Microsoft.Extensions.Options;

namespace Lemoncode.LibraryExample.FileStorage;

public class FileRepository : IFileRepository
{

	private readonly FileStorageRepositoryConfig _config;

	public FileRepository(IOptionsSnapshot<FileStorageRepositoryConfig> config)
	{
		_config = config.Value;
	}

	public (Stream Stream, string FileName) GetBookImage(int bookId)
	{
		var files = Directory.GetFiles(_config.FileStoragePath, $"{bookId}.*", SearchOption.TopDirectoryOnly);
		if (!files.Any())
		{
			throw new FileNotFoundException($"Unable to find the book image for the book with ID {bookId}.");
		}
		return (File.OpenRead(files[0]), Path.GetFileName(files[0]));
	}

	public async Task<string> UploadImageToTempFile(BookImageUpload bookImageUpload)
	{
		ArgumentNullException.ThrowIfNull(bookImageUpload, nameof(bookImageUpload));
		var tempFile = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(bookImageUpload.FileName));
		using var fstr = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Write);
		await bookImageUpload.BinaryData.CopyToAsync(fstr);
		bookImageUpload.BinaryData.Dispose();
		return Path.GetFileName(tempFile);
	}

	public bool TempImageExists(string tempFileName)
	{
		return File.Exists(Path.Combine(Path.GetTempPath(), tempFileName));
	}

	public void AssignImageToBook(int bookId, string tempFile)
	{
		ArgumentNullException.ThrowIfNull(tempFile, nameof(tempFile));

		var tempFilePath = Path.Combine(Path.GetTempPath(), tempFile);

		if (!File.Exists(tempFilePath))
		{
			throw new FileNotFoundException($"Unable to find the temp file {tempFile}.");
		}

		if (BookImageExist(bookId))
		{
			DeleteImage(bookId);
		}
		
		File.Copy(tempFilePath, Path.Combine(_config.FileStoragePath, bookId.ToString() + Path.GetExtension(tempFile)));
	}

	public bool BookImageExist(int bookId)
	{
		var files = Directory.GetFiles(_config.FileStoragePath, $"{bookId}.*", SearchOption.TopDirectoryOnly);
		return files.Any();
	}

	public void DeleteImage(int bookId)
	{
		var files = Directory.GetFiles(_config.FileStoragePath, $"{bookId}.*", SearchOption.TopDirectoryOnly);
		if (!files.Any())
		{
			throw new FileNotFoundException($"Unable to find the image for the book with iID {bookId}.");
		}

		File.Delete(files.First());
	}
}
