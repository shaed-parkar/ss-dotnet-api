namespace SS.NoteTests;

public class AddNewNoteTests : ControllerTest
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
    public async Task should_return_bad_request_when_validation_fails()
    {
        // arrange
        var authorId = _author.Id;
        var payload = new NewNoteDto(string.Empty, Priority.High);
        using var client = Application.CreateClient();

        // act
        var response = await client.PostAsJsonAsync(ApiUriFactory.Note.AddNewNotes(authorId), payload);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task should_return_not_found_when_nonexistent_author_id_is_provided()
    {
        // arrange
        var authorId = Guid.NewGuid();
        var payload = new NewNoteDto("Random Test for Failure", Priority.High);
        using var client = Application.CreateClient();

        // act
        var response = await client.PostAsJsonAsync(ApiUriFactory.Note.AddNewNotes(authorId), payload);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task should_return_created_and_new_note_when_payload_is_valid()
    {
        // arrange
        var authorId = _author.Id;
        var payload = new NewNoteDto("Random Test for Failure", Priority.High);
        using var client = Application.CreateClient();

        var expectedResult = new NewNoteDto(payload.Message, payload.Priority);
        // act
        var response = await client.PostAsJsonAsync(ApiUriFactory.Note.AddNewNotes(authorId), payload);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<NewNoteDto>();
        result.Should().BeEquivalentTo(expectedResult);
    }
}