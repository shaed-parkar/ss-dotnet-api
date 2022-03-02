using Microsoft.EntityFrameworkCore;
using SS.DAL.Queries.Core;
using SS.Domain;

namespace SS.DAL.Queries;

public class GetAllAuthorsQuery : IQuery
{
    
}

public class GetAllAuthorsQueryHandler : IQueryHandler<GetAllAuthorsQuery, List<Author>>
{
    private readonly AuthStoreContext _context;

    public GetAllAuthorsQueryHandler(AuthStoreContext context)
    {
        _context = context;
    }

    public Task<List<Author>> Handle(GetAllAuthorsQuery query)
    {
        return _context.Authors.AsNoTracking().ToListAsync();
    }
}