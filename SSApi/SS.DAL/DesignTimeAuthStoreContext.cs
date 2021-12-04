using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SS.DAL;

public class DesignTimeAuthStoreContext : IDesignTimeDbContextFactory<AuthStoreContext>
{
    public AuthStoreContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddUserSecrets("A5E0686B-1334-404B-811C-8C3F1E542EF1")
            .Build();
        var builder = new DbContextOptionsBuilder<AuthStoreContext>();
        builder.UseSqlServer(config.GetConnectionString("SSAuthStore"));
        var context = new AuthStoreContext(builder.Options);
        return context;
    }
}