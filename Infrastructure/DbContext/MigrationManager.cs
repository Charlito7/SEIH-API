using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using (var scope = webApp.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetService<AppDbContext>())
            {
                try
                {
                    var allMigrations = appContext!.Database.GetPendingMigrations();
                    if (allMigrations.Any())
                    {
                        appContext.Database.Migrate();
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        return webApp;
    }

    public static WebApplication Seeder(this WebApplication webApp)
    {
        ArgumentNullException.ThrowIfNull(webApp, nameof(webApp));

        using var scope = webApp.Services.CreateScope();

        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            ApplicationDbContextSeed.Initialize(context);
        }
        catch
        {

        }

        return webApp;
    }
}
