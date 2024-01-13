using Lemoncode.LibraryExample.FileStorage.Config;

using Microsoft.Extensions.Options;

namespace Lemoncode.LibraryExample.FileStorage;

public class FileRepository : IFileRepository
{
	private const string PermanentLocationUriSchema = "localfilelibrary";

	private const string TempLocationUriSchema = "tmpfilelibrary";

	private readonly FileStorageRepositoryConfig _config;

	public FileRepository(IOptionsSnapshot<FileStorageRepositoryConfig> config)
	{
		ArgumentNullException.ThrowIfNull(config, nameof(config));

		_config = config.Value;
	}

	public Stream GetFile(Uri fileUri)
	{
		ArgumentNullException.ThrowIfNull(fileUri, nameof(fileUri));
		ValidatePermanentUriSchema(fileUri);

		var filePath = Path.Combine(_config.FileStoragePath, fileUri.PathAndQuery);
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException($"Unable to find the file with name {fileUri} inside the repo folder.");
		}

		return File.OpenRead(filePath);
	}

	public async Task<Uri> UploadTempFile(Stream stream, string originalFileName)
	{
		ArgumentNullException.ThrowIfNull(stream, nameof(stream));
		ArgumentNullException.ThrowIfNull(originalFileName, nameof(originalFileName));

		var tempFile = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(originalFileName));
		using var fstr = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Write);
		await stream.CopyToAsync(fstr);
		stream.Dispose();
		return new Uri($"{TempLocationUriSchema}:{Path.GetFileName(tempFile)}");
	}

	public bool TempFileExists(Uri tempFileUri)
	{
		ArgumentNullException.ThrowIfNull(tempFileUri, nameof(tempFileUri));
		ValidateTempUriSchema(tempFileUri);

		return File.Exists(Path.Combine(Path.GetTempPath(), tempFileUri.AbsolutePath));
	}


	public Task<Uri> MoveFileToPermanentLocation(Uri tempFileUri, string permanentFileName)
	{
		ArgumentNullException.ThrowIfNull(tempFileUri, nameof(tempFileUri));
		ArgumentNullException.ThrowIfNull(permanentFileName, nameof(permanentFileName));

		if (!tempFileUri.Scheme.Equals(TempLocationUriSchema, StringComparison.InvariantCultureIgnoreCase))
		{
			throw new ArgumentException($"{nameof(tempFileUri)} should represent a {TempLocationUriSchema}: url schema.");
		}

		var tempFilePath = Path.Combine(Path.GetTempPath(), tempFileUri.AbsolutePath);

		if (!File.Exists(tempFilePath))
		{
			throw new FileNotFoundException($"Unable to find the temp file {tempFileUri}.");
		}

		var permanentFilePath = Path.Combine(_config.FileStoragePath, permanentFileName);

		File.Move(tempFilePath, permanentFilePath, true);
		return Task.FromResult(new Uri($"{PermanentLocationUriSchema}:{permanentFileName}"));
	}

	public bool FileExists(Uri fileUri)
	{
		ArgumentNullException.ThrowIfNull(fileUri, nameof(fileUri));
		return File.Exists(Path.Combine(_config.FileStoragePath, fileUri.AbsolutePath));
	}

	public void DeleteFile(Uri fileUri)
	{
		ArgumentNullException.ThrowIfNull(fileUri, nameof(fileUri));

		var filePath = GetFilePathForSchema(fileUri);

		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException($"Unable to find the file with URI {fileUri}.");
		}

		File.Delete(filePath);
	}

	private void ValidatePermanentUriSchema(Uri fileUri)
	{
		if (!fileUri.Scheme.Equals(PermanentLocationUriSchema, StringComparison.InvariantCultureIgnoreCase))
		{
			throw new ArgumentException($"{nameof(fileUri)} should represent a {PermanentLocationUriSchema}: url schema.");
		}
	}

	private void ValidateTempUriSchema(Uri tempFileUri)
	{
		if (tempFileUri.Scheme.Equals(TempLocationUriSchema, StringComparison.InvariantCultureIgnoreCase))
		{
			throw new ArgumentException($"The temp files should use an URL with {TempLocationUriSchema}: schema.");
		}
	}

	private string GetFilePathForSchema(Uri fileUri)
	{
		ArgumentNullException.ThrowIfNull(fileUri, nameof(fileUri));
		var path = fileUri.Scheme switch
		{
			TempLocationUriSchema => Path.GetTempPath(),
			PermanentLocationUriSchema => _config.FileStoragePath,
			_ => throw new ArgumentException($"The uri schema is not valid. It should be {TempLocationUriSchema} or {PermanentLocationUriSchema}.")
		};

		return Path.Combine(path, fileUri.AbsolutePath);
	}
}
