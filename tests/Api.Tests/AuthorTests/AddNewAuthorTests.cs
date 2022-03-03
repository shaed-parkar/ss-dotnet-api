namespace SS.AuthorTests;

public class AddNewAuthorTests : ControllerTest
{
    private AuthorDto _author;

    [SetUp]
    public void Setup()
    {
        _author = null;
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_author != null)
        {
            TestContext.WriteLine($"Removing author created during test {_author.Id}");
            await TestDataManager.RemoveAuthor(_author.Id);
        }
    }

    [Test]
    public async Task should_return_bad_request_when_payload_is_invalid()
    {
        // arrange
        var newAuthor = new NewAuthorDto(string.Empty, "Doe Test");
        var payload = ApiTestHelper.CreateJsonPayloadForRequest(newAuthor);
        using var client = Application.CreateClient();

        // act
        var response = await client.PostAsync(ApiUriFactory.Author.AddNewAuthor, payload);

        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().Contain(NewAuthorValidation.MissingFirstNameMessage);
    }

    [Test]
    public async Task should_return_okay_when_payload_is_valid()
    {
        // arrange
        var newAuthor = new NewAuthorDto("Jane", "Doe Test");
        var payload = ApiTestHelper.CreateJsonPayloadForRequest(newAuthor);
        using var client = Application.CreateClient();

        // act
        var response = await client.PostAsync(ApiUriFactory.Author.AddNewAuthor, payload);

        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseBody = await response.Content.ReadAsStringAsync();
        _author = JsonConvert.DeserializeObject<AuthorDto>(responseBody);
        _author!.FirstName.Should().Be(newAuthor.FirstName);
        _author.LastName.Should().Be(newAuthor.LastName);
        _author.Id.Should().NotBeEmpty();
    }
}