#pragma warning disable CS8618
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SS.Domain;


namespace SS.DAL;

public class AuthStoreContext : DbContext
{
    public DbSet<Author> Authors { get; set; }

    public AuthStoreContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}