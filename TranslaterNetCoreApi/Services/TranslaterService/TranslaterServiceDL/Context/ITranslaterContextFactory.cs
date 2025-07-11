namespace TranslaterServiceDL.Context
{
    public interface ITranslaterContextFactory
    {
        Task<ITranslaterContext> CreateDbContext();
    }
}