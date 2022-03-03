namespace SS.ValidationTests;

public class NewAuthorValidationTests
{
    private readonly NewAuthorValidation _validator = new();

    [Test]
    public void should_pass_validation_when_all_properties_are_set()
    {
        // arrange
        var author = new NewAuthorDto("John", "Doe");

        // act
        var result = _validator.Validate(author);

        // assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void should_fail_validation_when_all_first_name_is_not_set()
    {
        // arrange
        var author = new NewAuthorDto(null!, "Doe");

        // act
        var result = _validator.Validate(author);

        // assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.ErrorMessage == NewAuthorValidation.MissingFirstNameMessage).Should()
            .BeTrue();
    }

    [Test]
    public void should_fail_validation_when_all_last_name_is_not_set()
    {
        // arrange
        var author = new NewAuthorDto("John", null!);

        // act
        var result = _validator.Validate(author);

        // assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.ErrorMessage == NewAuthorValidation.MissingLastNameMessage).Should()
            .BeTrue();
    }
}