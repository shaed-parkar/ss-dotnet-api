namespace SS.Domain.Tests.NoteTests;

public class UpdateNoteContentTests
{
    private Note _note = null!;

    [SetUp]
    public void Setup()
    {
        _note = new Note("Buy milk", PriorityLevel.High);
    }

    [Test]
    public void should_update_note_content()
    {
        // arrange
        var newContent = "Buy a birthday card";

        // act
        _note.UpdateContent(newContent);

        // assert
        _note.Content.Should().Be(newContent);
    }

    [Test]
    public void should_throw_exception_when_updating_note_content_with_an_invalid_value()
    {
        // act
        var action = () => _note.UpdateContent(string.Empty);

        // assert
        action.Should().Throw<DomainRuleException>()
            .And.ValidationFailures.Count.Should().Be(1);
    }
}