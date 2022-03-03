namespace SS.AuthorTests;

public class GetAllAuthorsTests : ControllerTest
{
    private List<Author> _authors;

    [SetUp]
    public void Setup()
    {
        _authors = new List<Author>();
    }

    [TearDown]
    public async Task TearDown()
    {
        await TestDataManager.RemoveAuthors(_authors);
    }

    [Test]
    public async Task should_return_okay_with_list_of_authors()
    {
        // arrange
        _authors = await TestDataManager.SeedAuthors(3);
        using var client = Application.CreateClient();

        // act
        var response = await client.GetAsync(ApiUriFactory.Author.GetAllAuthors);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<AuthorDto>>();
        result.Should().BeEquivalentTo(_authors, options => options.Excluding(author => author.Notes));
    }
}