using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TranslaterServiceDL.Context;

namespace TranslaterServiceDL.Initializer
{
    public static class FirstLunchApplication
    {
        public static async Task InitilizationDatabase(IServiceProvider provider, string connectionString, string pathToDataSeed)
        {
            Console.WriteLine($"ConnectionString From TranslaterService {connectionString}");

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("CoonectiongString to database is null or empty");

            using (var scope = provider.CreateScope())
            {
                var factory = scope.ServiceProvider.GetRequiredService<ITranslaterContextFactory>();
                var context = await factory.CreateDbContext();

                if (!await CkeckConnectiongToDatabase(context))
                    throw new Exception("Failed to connect to database after 10 attempts");

                await Migration(context);
                await InitialInitialization.InitilizeDataFromJsonAsync(context, pathToDataSeed);
            }
        }

        public static async Task Migration(ITranslaterContext context)
        {
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                try
                {
                    await context.Database.MigrateAsync();
                }
                catch
                {
                    throw new Exception("Failed during migration");
                }
            }
        }

        public static async Task<bool> CkeckConnectiongToDatabase(ITranslaterContext context)
        {
            if (string.IsNullOrEmpty(context.Database.GetDbConnection().ConnectionString))
                throw new NullReferenceException("Connectionstring is null or empty");

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    await context.Database.CanConnectAsync();
                    return true;
                }
                catch
                {
                    await Task.Delay(3000);
                }
            }

            return false;
        }
    }
}
