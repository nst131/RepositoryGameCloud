using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TranslaterServiceDL.Context
{
    public class TranslaterFactory : IDesignTimeDbContextFactory<TranslaterContext>
    {
        public TranslaterContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("Connection_String");

            //Console.WriteLine(connectionString);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                //Console.WriteLine(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../TranslaterWebApi")));

                var config = new ConfigurationBuilder()
                    .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../TranslaterWebApi")))
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();

                connectionString = config.GetConnectionString("DefaultConnection");

                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("Connection string is not found in environment variable or appsettings.json");

                //Console.WriteLine(connectionString);
            }

            var optionsBuilder = new DbContextOptionsBuilder<TranslaterContext>();
            optionsBuilder.UseNpgsql(connectionString);

            var context = new TranslaterContext(optionsBuilder.Options);

            if (!context.Database.CanConnect())
                throw new InvalidOperationException("Unable to connect to the PostgreSQL database with the provided connection string");

            return context;
        }
    }
}
