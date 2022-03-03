namespace SS;

public static class TestDbBuilder
{
    public static DbContextOptions<AuthStoreContext> CreateAuthStoreDbOptions()
    {
        var configRootBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .AddUserSecrets(AppConstants.SecretKey);
        var configRoot = configRootBuilder.Build();

        var databaseConnectionString = configRoot.GetConnectionString(AppConstants.AuthStoreDbName);

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<AuthStoreContext>();
        dbContextOptionsBuilder.UseSqlServer(databaseConnectionString);

        return dbContextOptionsBuilder.Options;
    }
}