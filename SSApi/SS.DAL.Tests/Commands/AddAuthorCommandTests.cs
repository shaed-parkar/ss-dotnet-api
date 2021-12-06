using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SS.DAL.Commands;
using SS.Domain;

namespace SS.DAL.Tests.Commands;

public class AddAuthorCommandTests : DatabaseTestsBase
{
    private AddAuthorCommandHandler _handler = null!;
    private Author? _author;
    
    [SetUp]
    public void SetUp()
    {
        var context = new AuthStoreContext(AuthStoreDbContextOptions);
        _handler = new AddAuthorCommandHandler(context);
    }

    [Test]
    public async Task should_add_new_author()
    {
        // arrange
        var firstName = "James";
        var lastName = "Green";
        var command = new AddAuthorCommand(firstName, lastName);
        
        // act
        await _handler.Handle(command);

        // assert
        await using var db = new AuthStoreContext(AuthStoreDbContextOptions);
        _author = await db.Authors.FirstAsync(x => x.FirstName == firstName && x.LastName == lastName);
        _author.Should().NotBeNull();
        _author.FirstName.Should().Be(firstName);
        _author.LastName.Should().Be(lastName);
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
}