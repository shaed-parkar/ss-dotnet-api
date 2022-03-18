namespace SS.Validations;

public class NewNoteValidation : AbstractValidator<NewNoteDto>
{
    public static readonly string MissingContentMessage = "Content is required";
    public static readonly string MissingPriorityMessage = "PriorityLevel is required";

    public NewNoteValidation()
    {
        RuleFor(x => x.Message).NotEmpty().WithMessage(MissingContentMessage);
        RuleFor(x => x.Priority).NotEqual(Priority.Unknown).WithMessage(MissingPriorityMessage);
    }
}