namespace SS.Validations;

public class NewAuthorValidation : AbstractValidator<NewAuthorDto>
{
    public static readonly string MissingFirstNameMessage = "FirstName is required";
    public static readonly string MissingLastNameMessage = "LastName is required";

    public NewAuthorValidation()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage(MissingFirstNameMessage);
        RuleFor(x => x.LastName).NotEmpty().WithMessage(MissingLastNameMessage);
    }
}