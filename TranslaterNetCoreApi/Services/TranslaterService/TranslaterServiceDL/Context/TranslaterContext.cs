using Microsoft.EntityFrameworkCore;
using TranslaterServiceDL.Models;

namespace TranslaterServiceDL.Context
{
    public class TranslaterContext : DbContext, ITranslaterContext
    {
        public TranslaterContext(DbContextOptions<TranslaterContext> option) : base(option) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new KeywordConfiguration());
            modelBuilder.ApplyConfiguration(new TranslationConfiguration());
        }
    }

    public class TranslaterContextFactory : ITranslaterContextFactory
    {
        private readonly IDbContextFactory<TranslaterContext> factory;

        public TranslaterContextFactory(IDbContextFactory<TranslaterContext> factory)
        {
            this.factory = factory;
        }

        public async Task<ITranslaterContext> CreateDbContext()
        {
            return await factory.CreateDbContextAsync();
        }
    }
}
