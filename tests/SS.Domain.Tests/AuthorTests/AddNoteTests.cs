namespace SS.Domain.Tests.AuthorTests;

public class AddNoteTests
{
    private Author _author = null!;

    [SetUp]
    public void Setup()
    {
        _author = new Author("John", "Doe");
    }

    [Test]
    public void should_add_a_note_for_an_author()
    {
        // arrange
        var noteMessage = "Get milk";
        var priority = PriorityLevel.Medium;

        // act
        _author.AddNote(noteMessage, priority);

        // assert
        _author.Notes.Should().HaveCount(1);
        _author.Notes.Should().Contain(x => x.Content == noteMessage && x.Priority == priority);
    }
}