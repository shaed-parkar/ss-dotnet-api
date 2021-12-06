using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SS.Common;

namespace SS.DAL.Tests;

public abstract class DatabaseTestsBase
{
    private string _databaseConnectionString = null!;
    protected DbContextOptions<AuthStoreContext> AuthStoreDbContextOptions = null!;
    protected TestDataManager TestDataManager = null!;
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var configRootBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .AddUserSecrets(AppConstants.SecretKey);

        var configRoot = configRootBuilder.Build();
        _databaseConnectionString = configRoot.GetConnectionString(AppConstants.AuthStoreDbName);

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<AuthStoreContext>();
        dbContextOptionsBuilder.EnableSensitiveDataLogging();
        dbContextOptionsBuilder.UseSqlServer(_databaseConnectionString);
        dbContextOptionsBuilder.EnableSensitiveDataLogging();
        AuthStoreDbContextOptions = dbContextOptionsBuilder.Options;

        var context = new AuthStoreContext(AuthStoreDbContextOptions);
        context.Database.Migrate();
        
        TestDataManager = new TestDataManager(AuthStoreDbContextOptions);
    }
}