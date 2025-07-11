using AuthTrainingDL;
using AuthTrainingDL.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthTraining
{
    public static class InitialMigration
    {
        public static async void Migration(this WebApplication app, string connectiongString)
        {
            Console.WriteLine($"ConnectionString From TranslaterService {connectiongString}");

            if (string.IsNullOrEmpty(connectiongString))
                throw new Exception("CoonectiongString to dabase is null");

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AuthTrainingContext>();

                if (!await CkeckConnectiongToDatabase(context))
                    throw new Exception("Failed to connect to database after 10 attempts");


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

                var seeder = scope.ServiceProvider.GetRequiredService<DataSeed>();
                try
                {
                    await seeder.SeedDataAsync();
                }
                catch
                {
                    throw new Exception("Exception during initializing database of data");
                }

            }
        }

        public static async Task<bool> CkeckConnectiongToDatabase(AuthTrainingContext context)
        {
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
