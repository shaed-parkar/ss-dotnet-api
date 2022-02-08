using System;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SS.Api.Contracts;
using SS.Domain;

namespace Api.Tests.AuthorTests;

public class GetAuthorByIdTests : ControllerTest
{
    private Author _author;
    
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
        var response = await client.GetAsync(ApiUriFactory.Author.GetAuthorById(id));
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task should_return_okay_with_author_when_existing_author_id_is_provided()
    {
        // arrange
        _author = await TestDataManager.SeedAuthor();
        
        // act
        using var client = Application.CreateClient();
        var response = await client.GetAsync(ApiUriFactory.Author.GetAuthorById(_author.Id));
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.DeserialiseTo<AuthorDto>();

        result.Should().BeEquivalentTo(_author, options => options.Excluding(author => author.Notes));
    }
}