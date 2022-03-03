namespace SS;

public abstract class ControllerTest
{
    protected WebApplicationFactory<Program> Application = null!;
    protected TestDataManager TestDataManager = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var dbOptions = TestDbBuilder.CreateAuthStoreDbOptions();
        TestDataManager = new TestDataManager(dbOptions);

        Application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseKestrel().UseEnvironment("Development"));
    }
}