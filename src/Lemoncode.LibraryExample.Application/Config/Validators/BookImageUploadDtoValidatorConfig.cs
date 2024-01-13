namespace Lemoncode.LibraryExample.Application.Config.Validators
{
	public record class BookImageUploadDtoValidatorConfig
	{
		public static readonly string ConfigSection = "Validators:Books:BookUploadImageDto";

		public required int ImageMaxSizeInKBytes { get; set; }

		public required string[] SupportedImageTypes { get; set; }
	}
}
