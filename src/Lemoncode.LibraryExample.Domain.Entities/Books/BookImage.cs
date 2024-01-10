namespace Lemoncode.LibraryExample.Domain.Entities.Books;

public record class BookImage : ValueObject
{

    public string FileName { get; private set; }

    public string AltText { get; private set; }

    public BookImage(string fileName, string altText)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            AddValidationError("The image file name is mandatory.");
        }

        if (!fileName.Contains("."))
        {
            AddValidationError("The image file name should contain an extension.");
        }

        if (string.IsNullOrWhiteSpace(altText))
        {
            AddValidationError("The image alt text is mandatory. Think in accessibility!");
        }

        if (altText.Length < 10 || altText.Length > 1000)
        {
            AddValidationError("The alt text should contains between 10 and 1000 characters.");
        }

        Validate();

        this.FileName = fileName;
        this.AltText = altText;
    }
}
