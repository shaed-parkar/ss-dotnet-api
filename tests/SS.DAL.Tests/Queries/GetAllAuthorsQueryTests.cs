namespace SS.DAL.Tests.Queries;

public class GetAllAuthorsQueryTests : DatabaseTestsBase
{
    private List<Author> _authors;
    private GetAllAuthorsQueryHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        var context = new AuthStoreContext(AuthStoreDbContextOptions);
        _handler = new GetAllAuthorsQueryHandler(context);
        _authors = new List<Author>();
    }

    [TearDown]
    public async Task TearDown()
    {
        await TestDataManager.RemoveAuthors(_authors);
    }

    [Test]
    public async Task should_return_all_authors()
    {
        // arrange
        _authors = await TestDataManager.SeedAuthors(3);
        var query = new GetAllAuthorsQuery();

        // act
        var result = await _handler.Handle(query);

        // assert
        result.Should().BeEquivalentTo(_authors);
    }
}