using Lemoncode.LibraryExample.Domain.Entities.Exceptions;

namespace Lemoncode.LibraryExample.Domain.Entities.Authors;

public class Author : Entity
{

	public int Id { get; private set; }

	public string FirstName { get; private set; }

	public string LastName { get; private set; }

	public Author(int id, string firstName, string lastName)
	{
		this.Id = id;
		this.FirstName = firstName;
		this.LastName = lastName;
	}

	public void UPdateFirstName(string firstName)
	{
		this.FirstName = firstName;
		EnsureStateIsValid();
	}

	public void UpdateLastName(string lastName)
	{
		this.LastName = lastName;
		EnsureStateIsValid();
	}

	protected override void EnsureStateIsValid()
	{
		if (string.IsNullOrWhiteSpace(FirstName) || FirstName.Length > 100)
		{
			AddValidationError("First name should contains between 1 and 100 characters, and cannot be empty.");
		}

		if (string.IsNullOrWhiteSpace(LastName) || LastName.Length > 100)
		{
			AddValidationError("Last name should contains between 1 and 100 characters, and cannot be empty.");
		}

		Validate();
	}
}
