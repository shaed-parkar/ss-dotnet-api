namespace SS.Commands;

public class AddAuthorCommand : ICommand
{
    public AddAuthorCommand(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; }
    public string LastName { get; }

    public Author CreatedAuthor { get; internal set; }
}

public class AddAuthorCommandHandler : ICommandHandler<AddAuthorCommand>
{
    private readonly AuthStoreContext _context;

    public AddAuthorCommandHandler(AuthStoreContext context)
    {
        _context = context;
    }

    public async Task Handle(AddAuthorCommand command)
    {
        var author = new Author(command.FirstName, command.LastName);
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        command.CreatedAuthor = author;
    }
}