using Microsoft.EntityFrameworkCore;
using SS.DAL;
using SS.Domain;

namespace SS.Tests.Common;

public class TestDataManager
{
    private readonly DbContextOptions<AuthStoreContext> _dbContextOptions;

    public TestDataManager(DbContextOptions<AuthStoreContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    public async Task<Author> SeedAuthor()
    {
        var author = new Author("John", "Doe");
        await using var db = new AuthStoreContext(_dbContextOptions);
        db.Authors.Add(author);
        await db.SaveChangesAsync();
        return author;
    }

    public async Task RemoveAuthor(Guid authorId)
    {
        await using var db = new AuthStoreContext(_dbContextOptions);

        var author = await db.Authors.Include(x => x.Notes).FirstAsync(x => x.Id == authorId);
        
        db.Remove(author);
        await db.SaveChangesAsync();
    }

    public async Task SeedNotesForAuthor(Guid authorId, List<Note> notes)
    {
        await using var db = new AuthStoreContext(_dbContextOptions);

        var auth = await db.Authors.Include(x => x.Notes).FirstAsync(x => x.Id == authorId);
        foreach (var note in notes)
        {
            auth.AddNote(note.Content, note.Priority);
        }

        await db.SaveChangesAsync();
    }
}