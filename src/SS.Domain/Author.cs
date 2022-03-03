using SS.Core;
using SS.Enums;
using SS.Validations;

namespace SS;

public class Author : Entity<Guid>
{
    private readonly List<Note> _notes;

    public Author(string firstName, string lastName)
    {
        ValidateArguments(firstName, lastName);
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        _notes = new List<Note>();
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public IReadOnlyCollection<Note> Notes => _notes.AsReadOnly();

    public void UpdateName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("Missing first name", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Missing last name", nameof(lastName));
        FirstName = firstName;
        LastName = lastName;
    }

    public void AddNote(string content, PriorityLevel priority)
    {
        var note = new Note(content, priority);
        _notes.Add(note);
    }

    private void ValidateArguments(string firstName, string lastName)
    {
        var validationFailures = new ValidationFailures();
        if (string.IsNullOrWhiteSpace(firstName))
            validationFailures.AddFailure(nameof(firstName), "First name is missing");

        if (string.IsNullOrWhiteSpace(lastName))
            validationFailures.AddFailure(nameof(lastName), "Last name is missing");
        if (validationFailures.Any()) throw new DomainRuleException(validationFailures);
    }
}