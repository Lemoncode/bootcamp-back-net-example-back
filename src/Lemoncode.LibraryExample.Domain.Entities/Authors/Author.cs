namespace Lemoncode.LibraryExample.Domain.Entities.Authors;

public class Author
{
	private string _firstName = null!;
	private string _lastName = null!;

	public required int Id { get; set; }

	public required string FirstName
	{
		get => _firstName;
		set
		{
			if (value.Length < 1 || value.Length > 100)
			{
				throw new ArgumentOutOfRangeException("The author's first name must contains between 1 and 100 characters.");
			}

			_firstName = value;
		}
	}

	public required string LastName
	{
		get => _lastName;
		set
		{
			if (value.Length < 1 || value.Length > 100)
			{
				throw new ArgumentOutOfRangeException("The author's last name must contains between 1 and 100 characters.");
			}

			_lastName = value;
		}
	}
}
