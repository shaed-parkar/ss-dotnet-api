namespace SS.Domain.Tests.AuthorTests;

public class UpdateNameTests
{
    private Author _author = null!;

    [SetUp]
    public void Setup()
    {
        _author = new Author("John", "Doe");
    }

    [Test]
    public void should_update_author_name()
    {
        // arrange
        var newFirstName = "James";
        var newLastName = "Green";

        // act
        _author.UpdateName(newFirstName, newLastName);

        // assert
        _author.FirstName.Should().Be(newFirstName);
        _author.LastName.Should().Be(newLastName);
    }

    [Test]
    public void should_throw_exception_when_updating_author_name_without_firstname()
    {
        // act
        var action = () => _author.UpdateName(null!, "Green");

        // assert
        action.Should().Throw<ArgumentException>().Which.ParamName.Should().BeEquivalentTo(nameof(_author.FirstName));
    }

    [Test]
    public void should_throw_exception_when_updating_author_name_without_lastname()
    {
        // act
        var action = () => _author.UpdateName("James", null!);

        // assert
        action.Should().Throw<ArgumentException>().Which.ParamName.Should().BeEquivalentTo(nameof(_author.LastName));
    }
}