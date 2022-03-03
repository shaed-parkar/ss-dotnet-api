namespace SS.Queries;

public class GetAuthorByIdQueryTests : DatabaseTestsBase
{
    private Author _author;
    private GetAuthorByIdQueryHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        var context = new AuthStoreContext(AuthStoreDbContextOptions);
        _handler = new GetAuthorByIdQueryHandler(context);
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
    public async Task should_return_null_when_author_does_not_exist()
    {
        // arrange
        var authorId = Guid.NewGuid();
        var query = new GetAuthorByIdQuery(authorId);

        // act
        var result = await _handler.Handle(query);

        // assert
        result.Should().BeNull();
    }

    [Test]
    public async Task should_return_an_author_when_id_exists()
    {
        // arrange
        _author = await TestDataManager.SeedAuthor();
        var query = new GetAuthorByIdQuery(_author.Id);

        // act
        var result = await _handler.Handle(query);

        // assert
        result.Should().BeEquivalentTo(_author);
    }
}