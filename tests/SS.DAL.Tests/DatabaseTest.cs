namespace SS;

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