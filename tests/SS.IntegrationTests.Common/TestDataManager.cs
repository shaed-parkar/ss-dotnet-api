namespace SS;

public class TestDataManager
{
    private readonly DbContextOptions<AuthStoreContext> _dbContextOptions;

    public TestDataManager(DbContextOptions<AuthStoreContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    public async Task<List<Author>> SeedAuthors(int numberOfAuthors)
    {
        await using var db = new AuthStoreContext(_dbContextOptions);

        var authors = new List<Author>();
        for (var i = 0; i < numberOfAuthors; i++)
        {
            var suffix = i + 1;
            authors.Add(new Author($"John{suffix}", $"Doe{suffix}"));
        }

        db.Authors.AddRange(authors);
        await db.SaveChangesAsync();
        return authors;
    }

    public async Task<Author> SeedAuthor()
    {
        var author = new Author("John", "Doe");
        await using var db = new AuthStoreContext(_dbContextOptions);
        db.Authors.Add(author);
        await db.SaveChangesAsync();
        return author;
    }

    public async Task RemoveAuthors(List<Author> authors)
    {
        await using var db = new AuthStoreContext(_dbContextOptions);

        foreach (var author in authors)
        {
            var entity = await db.Authors.Include(x => x.Notes).FirstAsync(x => x.Id == author.Id);
            db.Remove(entity);
        }

        await db.SaveChangesAsync();
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
        foreach (var note in notes) auth.AddNote(note.Content, note.Priority);

        await db.SaveChangesAsync();
    }
}