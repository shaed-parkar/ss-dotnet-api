#pragma warning disable CS8618
using System.Reflection;
namespace SS;

public class AuthStoreContext : DbContext
{
    public AuthStoreContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Note> Notes     { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}