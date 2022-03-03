namespace Api.Extensions;

public static class AppBuilderDbExtensions
{
    /// <summary>
    ///     Run pending migrations
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder" /> instance</param>
    public static void RunLatestMigrations(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var ctx = serviceScope.ServiceProvider.GetService<AuthStoreContext>();
        ctx!.Database.Migrate();
    }
}