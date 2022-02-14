using System.Threading.Tasks;
using Api.Controllers;
using Autofac.Extras.Moq;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SS.DAL.Queries;
using SS.DAL.Queries.Core;
using SS.Domain;
using SS.Tests.Common;

namespace Api.Tests.Mocked.AuthorTests;

public class GetAuthorByIdTests
{
    private AutoMock _mocker;
    private AuthorsController _controller;

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