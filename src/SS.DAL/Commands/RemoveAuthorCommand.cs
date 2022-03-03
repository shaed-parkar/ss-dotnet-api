using SS.Commands.Core;
using SS.Exceptions;

namespace SS.Commands;

public class RemoveAuthorCommand : ICommand
{
    public RemoveAuthorCommand(Guid authorId)
    {
        AuthorId = authorId;
    }

    public Guid AuthorId { get; }
}

public class RemoveAuthorCommandHandler : ICommandHandler<RemoveAuthorCommand>
{
    private readonly AuthStoreContext _context;

    public RemoveAuthorCommandHandler(AuthStoreContext context)
    {
        _context = context;
    }

    public async Task Handle(RemoveAuthorCommand command)
    {
        var author = await _context.Authors.Include(x => x.Notes).FirstOrDefaultAsync(x => x.Id == command.AuthorId);
        if (author is null) throw new AuthorNotFoundException(command.AuthorId);
        _context.Remove(author);
        await _context.SaveChangesAsync();
    }
}