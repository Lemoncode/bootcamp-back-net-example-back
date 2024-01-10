namespace Lemoncode.LibraryExample.Domain.Entities.Books;

public record class BookDescription : ValueObject
{

    public string Description { get; private set; }

    public BookDescription(string description)
    {
        if (description is null || description.Length < 10 || description.Length > 4000)
        {
            AddValidationError("The description should contains between 10 and 4000 characters.");
        }

        Validate();

        this.Description = description ?? throw new ArgumentNullException(nameof(BookDescription));
    }
}