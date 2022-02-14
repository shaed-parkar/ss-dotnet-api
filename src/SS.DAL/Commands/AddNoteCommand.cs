using SS.DAL.Commands.Core;
using SS.DAL.Exceptions;
using SS.Domain.Enums;

namespace SS.DAL.Commands;

public class AddNoteCommand : ICommand
{
    public AddNoteCommand(Guid authorId, string content, PriorityLevel priorityLevel)
    {
        AuthorId = authorId;
        Content = content;
        PriorityLevel = priorityLevel;
    }

    public Guid AuthorId { get; }
    public string Content { get; }
    public PriorityLevel PriorityLevel { get; }
}

public class AddNoteCommandHandler : ICommandHandler<AddNoteCommand>
{
    private readonly AuthStoreContext _context;

    public AddNoteCommandHandler(AuthStoreContext context)
    {
        _context = context;
    }

    public async Task Handle(AddNoteCommand command)
    {
        var author = await _context.Authors.FindAsync(command.AuthorId);
        if (author == null)
        {
            throw new AuthorNotFoundException(command.AuthorId);
        }
        author.AddNote(command.Content, command.PriorityLevel);
        await _context.SaveChangesAsync();
    }
}