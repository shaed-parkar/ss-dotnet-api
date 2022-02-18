using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.Insights;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Sql;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;
using SkuArgs = Pulumi.AzureNative.Sql.Inputs.SkuArgs;

class AppServiceStack : Stack
{
    [Output]
    public Output<string> Endpoint { get; set; }
    
    public AppServiceStack()
    {
        var currentUser = Output.Create(GetClientConfig.InvokeAsync());
    
        var resourceGroup = new ResourceGroup("ss-dotnet-api-rg");

        var appServicePlan = new AppServicePlan("ss-dotnet-api-asp", new AppServicePlanArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Kind = "App",
            Sku = new SkuDescriptionArgs
            {
                Tier = "Free",
                Name = "F1",
            },
        });

        var appInsights = new Component("ss-dotnet-api-appInsights", new ComponentArgs
        {
            ApplicationType = "web",
            Kind = "web",
            ResourceGroupName = resourceGroup.Name,
        });

        var config = new Config();
        var username = config.Get("sqlAdmin") ?? "pulumi";
        var password = config.RequireSecret("sqlPassword");
        var sqlServer = new Server("ss-dotnet-api-sqlserver", new ServerArgs
        {
            ResourceGroupName = resourceGroup.Name,
            AdministratorLogin = username,
            AdministratorLoginPassword = password,
            Version = "12.0",
        });

        var database = new Database("ss-dotnet-api-db", new DatabaseArgs
        {
            ResourceGroupName = resourceGroup.Name,
            ServerName = sqlServer.Name,
            Sku = new SkuArgs
            {
                Name =  "S0"
            }
        });

        var app = new WebApp("ss-dotnet-api", new WebAppArgs
        {
            ResourceGroupName = resourceGroup.Name,
            ServerFarmId = appServicePlan.Id,
            SiteConfig = new SiteConfigArgs
            {
                AppSettings = {
                    new NameValuePairArgs{
                        Name = "APPINSIGHTS_INSTRUMENTATIONKEY",
                        Value = appInsights.InstrumentationKey
                    },
                    new NameValuePairArgs{
                        Name = "APPLICATIONINSIGHTS_CONNECTION_STRING",
                        Value = appInsights.InstrumentationKey.Apply(key => $"InstrumentationKey={key}"),
                    },
                    new NameValuePairArgs{
                        Name = "ApplicationInsightsAgent_EXTENSION_VERSION",
                        Value = "~2",
                    },
                },
                ConnectionStrings = {
                    new ConnStringInfoArgs
                    {
                        Name = "db",
                        Type = ConnectionStringType.SQLAzure,
                        ConnectionString = Output.Tuple(sqlServer.Name, database.Name, password).Apply(t =>
                        {
                            (string server, string dbName, string pwd) = t;
                            return
                                $"Server= tcp:{server}.database.windows.net;initial catalog={dbName};userID={username};password={pwd};Min Pool Size=0;Max Pool Size=30;Persist Security Info=true;";
                        }),
                    },
                },
            }
        });

        Endpoint = app.DefaultHostName;
    }
}