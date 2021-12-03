using System;
using FluentAssertions;
using NUnit.Framework;
using SS.Domain.Enums;
using SS.Domain.Validations;

namespace SS.Domain.Tests.NoteTests;

public class CreateNoteTests
{
    [Test]
    public void should_create_a_note_with_valid_params()
    {
        // arrange
        var content = "Buy milk";
        var priority = PriorityLevel.High;
        
        // act
        var note = new Note(content, priority);
        
        // assert
        note.Content.Should().Be(content);
        note.Priority.Should().Be(priority);
    }

    [Test]
    public void should_throw_exception_when_creating_a_note_without_content()
    {
        // act
        Action action = () => new Note(null!, PriorityLevel.High);
        
        // assert
        action.Should().Throw<DomainRuleException>()
            .And.ValidationFailures.Count.Should().Be(1);
    }
}