using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using SS.Tests.Common;

namespace Api.Tests;

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