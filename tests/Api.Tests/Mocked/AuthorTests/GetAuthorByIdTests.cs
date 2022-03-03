namespace Api.Tests.Mocked.AuthorTests;

public class GetAuthorByIdTests
{
    private AuthorsController _controller;
    private AutoMock _mocker;

    [SetUp]
    public void Setup()
    {
        _mocker = AutoMock.GetLoose();
        _controller = _mocker.Create<AuthorsController>();
    }

    [Test]
    public async Task should_return_author_and_ok_status()
    {
        // arrange
        var author = new Builder(DefaultBuilderSettings.Instance()).CreateNew<Author>()
            .WithFactory(() => new Author("John", "Doe")).Build();
        _mocker.Mock<IQueryHandler>()
            .Setup(x => x.Handle<GetAuthorByIdQuery, Author>(It.Is<GetAuthorByIdQuery>(q => q.AuthorId == author.Id)))
            .ReturnsAsync(author);

        // act
        var result = await _controller.GetAuthorById(author.Id);

        // assert
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(author, o => o.Excluding(x => x.Notes));
    }
}