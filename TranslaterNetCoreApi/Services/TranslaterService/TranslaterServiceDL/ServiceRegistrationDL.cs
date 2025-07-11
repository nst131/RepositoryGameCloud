using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TranslaterServiceDL.Context;
using TranslaterServiceDL.Initializer;

namespace TranslaterServiceDL
{
    public static class ServiceRegistrationDL
    {
        public static void AddRegistrationDL(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextFactory<TranslaterContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<ITranslaterContextFactory, TranslaterContextFactory>();
        }
    }
}
