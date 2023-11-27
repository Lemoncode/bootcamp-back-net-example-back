using Lemoncode.LibraryExample.Domain.Abstractions.Repositories;
using Lemoncode.LibraryExample.Domain.Entities.Books;
using Lemoncode.LibraryExample.FileStorage.Config;

using Microsoft.Extensions.Options;

namespace Lemoncode.LibraryExample.FileStorage;

public class BookImageRepository : IBookImageRepository
{

	private readonly BookImageRepositoryConfig _config;

	public BookImageRepository(IOptionsSnapshot<BookImageRepositoryConfig> config)
	{
		_config = config.Value;
	}

	public (Stream Stream, string FileName) GetBookImage(int bookId)
	{
		var files = Directory.GetFiles(_config.ImageStoragePath, $"{bookId}.*", SearchOption.TopDirectoryOnly);
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

	public bool BookImageExists(string tempFileName)
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

		File.Copy(tempFilePath, Path.Combine(_config.ImageStoragePath, bookId.ToString() + Path.GetExtension(tempFile)));
	}

}
