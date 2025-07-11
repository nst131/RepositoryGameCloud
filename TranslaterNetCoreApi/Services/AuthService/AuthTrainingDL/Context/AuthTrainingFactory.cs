using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuthTrainingDL.Context
{
    public class AuthTrainingFactory : IDesignTimeDbContextFactory<AuthTrainingContext>
    {
        public AuthTrainingContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("Connection_String");

            //Console.WriteLine(connectionString);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                //Console.WriteLine(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\AuthTraining")));

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\AuthTraining")))
                    .AddJsonFile("appsettings.json")
                    .Build();

                connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("Connection string is not found in environment variable or appsettings.json");

                //Console.WriteLine(connectionString);
            }

            var optionsBuilder = new DbContextOptionsBuilder<AuthTrainingContext>();
            optionsBuilder.UseNpgsql(connectionString);

            var context = new AuthTrainingContext(optionsBuilder.Options);

            if (!context.Database.CanConnect())
                throw new InvalidOperationException("Unable to connect to the PostgreSQL database with the provided connection string");

            return context;
        }
    }
}
