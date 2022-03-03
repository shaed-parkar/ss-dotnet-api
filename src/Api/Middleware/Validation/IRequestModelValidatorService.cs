namespace SS.Middleware.Validation;

public interface IRequestModelValidatorService
{
    IList<ValidationFailure> Validate(Type requestModel, object modelValue);
}