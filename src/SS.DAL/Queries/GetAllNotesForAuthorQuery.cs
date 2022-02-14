using Microsoft.EntityFrameworkCore;
using SS.DAL.Exceptions;
using SS.DAL.Queries.Core;
using SS.Domain;

namespace SS.DAL.Queries;

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
        if (author == null)
        {
            throw new AuthorNotFoundException(query.AuthorId);
        }

        return author.Notes;
    }
}