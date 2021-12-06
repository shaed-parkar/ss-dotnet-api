using SS.DAL.Commands.Core;
using SS.Domain;

namespace SS.DAL.Commands;

public class AddAuthorCommand : ICommand
{
    public AddAuthorCommand(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; }
    public string LastName { get; }
}

public class AddAuthorCommandHandler : ICommandHandler<AddAuthorCommand>
{
    private readonly AuthStoreContext _context;

    public AddAuthorCommandHandler(AuthStoreContext context)
    {
        _context = context;
    }

    public Task Handle(AddAuthorCommand command)
    {
        var author = new Author(command.FirstName, command.LastName);
        _context.Authors.Add(author);
        return _context.SaveChangesAsync();
    }
}