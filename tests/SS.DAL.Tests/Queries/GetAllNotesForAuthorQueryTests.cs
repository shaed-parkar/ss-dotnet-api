namespace SS.DAL.Tests.Queries;

public class GetAllNotesForAuthorQueryTests : DatabaseTestsBase
{
    private Author _author;
    private GetAllNotesForAuthorQueryHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        var context = new AuthStoreContext(AuthStoreDbContextOptions);
        _handler = new GetAllNotesForAuthorQueryHandler(context);
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
    public void should_throw_exception_when_getting_notes_for_a_nonexistent_author()
    {
        // arrange
        var authorId = Guid.NewGuid();
        var query = new GetAllNotesForAuthorQuery(authorId);

        // act
        var f = async () => { await _handler.Handle(query); };

        // assert
        f.Should().ThrowAsync<AuthorNotFoundException>().Result
            .And.AuthorId.Should().Be(authorId);
    }

    [Test]
    public async Task should_return_all_notes_for_an_author()
    {
        // arrange
        _author = await TestDataManager.SeedAuthor();
        var notes = new Builder(DefaultBuilderSettings.Instance()).CreateListOfSize<Note>(5).Build().ToList();
        await TestDataManager.SeedNotesForAuthor(_author.Id, notes);

        var query = new GetAllNotesForAuthorQuery(_author.Id);

        // act
        var results = await _handler.Handle(query);

        // assert
        results.Should().NotBeNullOrEmpty();
        results.Should().BeEquivalentTo(notes, options => options.Excluding(n => n.Id));
    }
}