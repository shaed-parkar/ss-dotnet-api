using SS.Domain.Core;
using SS.Domain.Enums;
using SS.Domain.Validations;

namespace SS.Domain;

public class Note : Entity<int>
{
    protected Note()
    {
    }

    public Note(string content, PriorityLevel priority)
    {
        ValidateArguments(content);
        Content = content;
        Priority = priority;
    }

    public string Content { get; private set; }
    public PriorityLevel Priority { get; }

    public void UpdateContent(string content)
    {
        ValidateArguments(content);
        Content = content;
    }

    private void ValidateArguments(string content)
    {
        var validationFailures = new ValidationFailures();
        if (string.IsNullOrWhiteSpace(content))
            validationFailures.AddFailure(nameof(content), "A note must have content");
        if (validationFailures.Any()) throw new DomainRuleException(validationFailures);
    }
}