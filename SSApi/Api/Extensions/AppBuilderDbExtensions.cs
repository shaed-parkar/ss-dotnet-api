using Microsoft.EntityFrameworkCore;
using SS.DAL;

namespace Api.Extensions;

public static class AppBuilderDbExtensions
{
    public static void RunLatestMigrations(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var ctx = serviceScope.ServiceProvider.GetService<AuthStoreContext>();
        ctx!.Database.Migrate();
    }
}