using System;
using FluentAssertions;
using NUnit.Framework;
using SS.Domain.Validations;

namespace SS.Domain.Tests.AuthorTests;

public class CreatePersonTests
{
    [Test]
    public void should_create_an_author_with_valid_params()
    {
        // act
        var author =  new Author("John", "Doe");
        
        // assert
        author.FirstName.Should().Be("John");
        author.LastName.Should().Be("Doe");
    }

    [Test]
    public void should_throw_exception_when_creating_an_author_without_a_firstname()
    {
        // act
        Action action = () => new Author(string.Empty, "Doe");
        
        // assert
        action.Should().Throw<DomainRuleException>()
            .And.ValidationFailures.Count.Should().Be(1);
    }
    
    [Test]
    public void should_throw_exception_when_creating_an_author_without_a_lastname()
    {
        // act
        Action action = () => new Author("John", null!);
        
        // assert
        action.Should().Throw<DomainRuleException>()
            .And.ValidationFailures.Count.Should().Be(1);
    }
}