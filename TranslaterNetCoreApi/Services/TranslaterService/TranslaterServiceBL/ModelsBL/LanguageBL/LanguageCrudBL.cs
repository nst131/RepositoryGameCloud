using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TranslaterServiceBL.Common.Exceptions;
using TranslaterServiceBL.ModelsBL.LanguageBL.Dto;
using TranslaterServiceDL.Context;
using TranslaterServiceDL.Models;

namespace TranslaterServiceBL.ModelsBL.LanguageBL
{
    public class LanguageCrudBL : ILanguageCrudBL
    {
        private readonly ITranslaterContextFactory contextFactory;
        private readonly IMapper mapper;
        public LanguageCrudBL(
            ITranslaterContextFactory contextFactory,
            IMapper mapper)
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
        }

        public async Task<ICollection<OutLaguageDtoBL>> GetAllAsync(
            CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            await using var context = await contextFactory.CreateDbContext();

            var languages = await context.Set<Language>().AsNoTracking().ToListAsync(token);

            if (languages is null)
                return new List<OutLaguageDtoBL>();

            return languages.Select(x => mapper.Map<OutLaguageDtoBL>(x)).ToList();
        }

        public async Task InsertNewLanguageAsync(
            InputInsertNewLanguageDtoBL input,
            CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            await using var context = await contextFactory.CreateDbContext();

            if (string.IsNullOrEmpty(input.Name))
                throw new DtoVereficationException($"{input.Name} of language is empty or null");

            if (context.Set<Language>().Any(x => x.Name == input.Name))
                throw new DtoVereficationException($"{input.Name} of language is existed yet");

            var keywordsId = await context.Set<Keyword>().Select(x => x.Id).ToListAsync();

            await context.Set<Language>().AddAsync(new Language()
            {
                Name = input.Name,
                Translations = keywordsId.Select(x => new Translation() { KeywordId = x }).ToList()
            });
            await context.SaveChangesAsync();
        }

        public async Task DeleteLanguageAsync(
            Guid id,
            CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            await using var context = await contextFactory.CreateDbContext();

            var result = await context.Set<Language>()
                .Select(x => new Language() { Id = x.Id })
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(token);

            if (result is null)
                throw new DtoVereficationException($"{nameof(Language)} does not exist by specified id");

            context.Set<Language>().Remove(result);
            await context.SaveChangesAsync();
        }
    }

    public interface ILanguageCrudBL
    {
        Task DeleteLanguageAsync(Guid id, CancellationToken token);
        Task<ICollection<OutLaguageDtoBL>> GetAllAsync(CancellationToken token);
        Task InsertNewLanguageAsync(InputInsertNewLanguageDtoBL input, CancellationToken token);
    }
}
