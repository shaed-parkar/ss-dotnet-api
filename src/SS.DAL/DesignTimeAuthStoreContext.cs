namespace SS.DAL;

public class DesignTimeAuthStoreContext : IDesignTimeDbContextFactory<AuthStoreContext>
{
    public AuthStoreContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddUserSecrets(AppConstants.SecretKey)
            .Build();
        var builder = new DbContextOptionsBuilder<AuthStoreContext>();
        builder.UseSqlServer(config.GetConnectionString(AppConstants.AuthStoreDbName));
        var context = new AuthStoreContext(builder.Options);
        return context;
    }
}