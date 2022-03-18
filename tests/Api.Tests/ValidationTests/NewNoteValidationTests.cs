namespace SS.ValidationTests;

public class NewNoteValidationTests
{
    private readonly NewNoteValidation _validator = new();

    [Test]
    public void should_pass_validation_when_all_properties_are_acceptable()
    {
        // arrange
        var note = new NewNoteDto("Hello", Priority.High);

        // act
        var result = _validator.Validate(note);

        // assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void should_fail_validation_when_content_is_not_set()
    {
        // arrange
        var note = new NewNoteDto(string.Empty, Priority.High);

        // act
        var result = _validator.Validate(note);

        // assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.ErrorMessage == NewNoteValidation.MissingContentMessage).Should()
            .BeTrue();
    }

    [Test]
    public void should_fail_validation_when_priority_level_is_not_set()
    {
        // arrange
        var note = new NewNoteDto("Hello", Priority.Unknown);

        // act
        var result = _validator.Validate(note);

        // assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.ErrorMessage == NewNoteValidation.MissingPriorityMessage).Should()
            .BeTrue();
    }
}