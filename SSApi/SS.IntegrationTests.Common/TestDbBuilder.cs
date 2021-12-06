using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SS.Common;
using SS.DAL;

namespace SS.Tests.Common;

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
        dbContextOptionsBuilder.EnableSensitiveDataLogging();
        dbContextOptionsBuilder.UseSqlServer(databaseConnectionString);
        
        return dbContextOptionsBuilder.Options;
    }
}