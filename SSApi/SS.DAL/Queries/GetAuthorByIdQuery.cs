using Microsoft.EntityFrameworkCore;
using SS.DAL.Queries.Core;
using SS.Domain;

namespace SS.DAL.Queries;

public class GetAuthorByIdQuery : IQuery
{
    public GetAuthorByIdQuery(Guid authorId)
    {
        AuthorId = authorId;
    }

    public Guid AuthorId { get; }
}

public class GetAuthorByIdQueryHandler : IQueryHandler<GetAuthorByIdQuery, Author>
{
    private readonly AuthStoreContext _context;

    public GetAuthorByIdQueryHandler(AuthStoreContext context)
    {
        _context = context;
    }

    public Task<Author> Handle(GetAuthorByIdQuery query)
    {
        return _context.Authors.AsNoTracking()
            .Include(x => x.Notes)
            .FirstOrDefaultAsync(x => x.Id == query.AuthorId);
    }
}