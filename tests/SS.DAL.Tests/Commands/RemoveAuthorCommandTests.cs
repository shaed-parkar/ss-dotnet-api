namespace SS.Commands;

public class RemoveAuthorCommandTests : DatabaseTestsBase
{
    private Author _author;
    private RemoveAuthorCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        var context = new AuthStoreContext(AuthStoreDbContextOptions);
        _handler = new RemoveAuthorCommandHandler(context);
    }

    [TearDown]
    public async Task TearDown()
    {
        // only a precaution if remove fails
        if (_author is not null)
        {
            TestContext.WriteLine($"Removing test author {_author.Id}");
            await TestDataManager.RemoveAuthor(_author.Id);
        }
    }

    [Test]
    public void should_throw_exception_when_author_does_not_exist()
    {
        // arrange
        var authorId = Guid.NewGuid();
        var command = new RemoveAuthorCommand(authorId);

        // act
        var f = async () => { await _handler.Handle(command); };

        // assert
        f.Should().ThrowAsync<AuthorNotFoundException>();
    }

    [Test]
    public async Task should_remove_existing_author_when_author_exists()
    {
        // arrange
        _author = await TestDataManager.SeedAuthor();
        var command = new RemoveAuthorCommand(_author.Id);

        // act

        await _handler.Handle(command);

        // assert
        await using var db = new AuthStoreContext(AuthStoreDbContextOptions);
        var result = await db.Authors.FindAsync(_author.Id);
        result.Should().BeNull();

        // set to null so clean up does not try again
        _author = null;
    }
}