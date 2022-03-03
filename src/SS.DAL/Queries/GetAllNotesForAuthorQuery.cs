using SS.Exceptions;
using SS.Queries.Core;

namespace SS.Queries;

public class GetAllNotesForAuthorQuery : IQuery
{
    public GetAllNotesForAuthorQuery(Guid authorId)
    {
        AuthorId = authorId;
    }

    public Guid AuthorId { get; }
}

public class GetAllNotesForAuthorQueryHandler : IQueryHandler<GetAllNotesForAuthorQuery, IReadOnlyCollection<Note>>
{
    private readonly AuthStoreContext _context;

    public GetAllNotesForAuthorQueryHandler(AuthStoreContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Note>> Handle(GetAllNotesForAuthorQuery query)
    {
        var author = await _context.Authors.AsNoTracking()
            .Include(x => x.Notes)
            .FirstOrDefaultAsync(x => x.Id == query.AuthorId);
        if (author == null) throw new AuthorNotFoundException(query.AuthorId);

        return author.Notes;
    }
}