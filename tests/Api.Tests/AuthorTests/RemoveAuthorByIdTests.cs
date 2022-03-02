namespace Api.Tests.AuthorTests;

public class RemoveAuthorByIdTests : ControllerTest
{
    private Author _author;
    
    [SetUp]
    public void SetUp()
    {
        _author = null;
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
    public async Task should_return_not_found_when_nonexistent_id_is_provided()
    {
        // arrange
        var id = Guid.NewGuid();

        // act
        using var client = Application.CreateClient();
        var response = await client.DeleteAsync(ApiUriFactory.Author.RemoveAuthorById(id));

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task should_return_bad_request_when_id_is_invalid_type()
    {
        // arrange
        using var client = Application.CreateClient();
        
        // act
        var response = await client.DeleteAsync(ApiUriFactory.Author.RemoveAuthorById(Guid.Empty));

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task should_return_okay_when_author_has_been_removed()
    {
        // arrange
        _author = await TestDataManager.SeedAuthor();
        using var client = Application.CreateClient();
        
        // act
        var response = await client.DeleteAsync(ApiUriFactory.Author.RemoveAuthorById(_author.Id));
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // set to null so clean up does not try again
        _author = null;
    }
}