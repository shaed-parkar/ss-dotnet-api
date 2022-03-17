namespace SS.NoteTests;

public class GetAllNotesForAuthorTests : ControllerTest
{
    private Author _author;

    [SetUp]
    public async Task Setup()
    {
        _author = await TestDataManager.SeedAuthor();
    }

    [TearDown]
    public async Task TearDown()
    {
        TestContext.WriteLine($"Removing author created during test {_author.Id}");
        await TestDataManager.RemoveAuthor(_author.Id);
    }

    [Test]
    public async Task should_return_not_found_when_nonexistent_author_id_is_provided()
    {
        // arrange
        var authorId = Guid.NewGuid();
        using var client = Application.CreateClient();

        // act
        var response = await client.GetAsync(ApiUriFactory.Note.GetAllNotesForAuthor(authorId));

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task should_okay_with_notes_for_an_author_when_id_for_author_exists()
    {
        // arrange
        var authorId = _author.Id;
        var notes = new Builder(DefaultBuilderSettings.Instance()).CreateListOfSize<Note>(5).Build().ToList();
        await TestDataManager.SeedNotesForAuthor(_author.Id, notes);
        using var client = Application.CreateClient();

        var expectedResult = notes.Select(x => new NoteDto(x.Content, (Priority) x.Priority));

        // act
        var response = await client.GetAsync(ApiUriFactory.Note.GetAllNotesForAuthor(authorId));

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<NoteDto>>();
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(expectedResult);
    }
}