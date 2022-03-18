namespace SS.Commands;

public class AddNewNoteCommandTests : DatabaseTestsBase
{
    private Author _author;
    private AddNoteCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        var context = new AuthStoreContext(AuthStoreDbContextOptions);
        _handler = new AddNoteCommandHandler(context);
        _author = null;
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_author != null)
        {
            TestContext.WriteLine($"Removing test author {_author.Id}");
            await TestDataManager.RemoveAuthor(_author.Id);
        }
    }

    [Test]
    public void should_throw_exception_when_adding_note_for_author_that_does_not_exist()
    {
        // arrange
        var newNoteCommand = new AddNoteCommand(Guid.NewGuid(), "Test", PriorityLevel.High);

        // act
        var f = async () => { await _handler.Handle(newNoteCommand); };

        // assert
        f.Should().ThrowAsync<AuthorNotFoundException>();
    }

    [Test]
    public async Task should_add_new_note_to_author()
    {
        // arrange
        _author = await TestDataManager.SeedAuthor();
        var content = "Test Auto";
        var priorityLevel = PriorityLevel.Low;
        var command = new AddNoteCommand(_author.Id, content, priorityLevel);

        // act
        await _handler.Handle(command);

        // assert
        await using var db = new AuthStoreContext(AuthStoreDbContextOptions);
        var result = await db.Authors.Include(x => x.Notes).FirstAsync(x => x.Id == _author.Id);
        result.Notes.Should().Contain(note => note.Content == content && note.Priority == priorityLevel);
        var createdNote = command.CreatedNote;
        createdNote.Should().BeEquivalentTo(new Note(content, priorityLevel), options => options.Excluding(x => x.Id));
    }
}