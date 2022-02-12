using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SS.Common;
using SS.Tests.Common;

namespace SS.DAL.Tests;

public abstract class DatabaseTestsBase
{
    protected DbContextOptions<AuthStoreContext> AuthStoreDbContextOptions = null!;
    protected TestDataManager TestDataManager = null!;
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        AuthStoreDbContextOptions = TestDbBuilder.CreateAuthStoreDbOptions();

        var context = new AuthStoreContext(AuthStoreDbContextOptions);
        context.Database.Migrate();
        
        TestDataManager = new TestDataManager(AuthStoreDbContextOptions);
    }
}