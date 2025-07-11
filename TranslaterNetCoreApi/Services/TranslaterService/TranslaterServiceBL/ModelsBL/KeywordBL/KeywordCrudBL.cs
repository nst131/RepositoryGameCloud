using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TranslaterServiceBL.Common;
using TranslaterServiceBL.Common.Exceptions;
using TranslaterServiceBL.ModelsBL.KeywordBL.Dto;
using TranslaterServiceDL.Context;
using TranslaterServiceDL.Models;

namespace TranslaterServiceBL.ModelsBL.KeywordBL
{
    public class KeywordCrudBL : IKeywordCrudBL
    {
        private readonly ITranslaterContextFactory contextFactory;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly ValidatorDto<InputKeywordByValueDtoBL, bool> validationKeywordByValue;

        public KeywordCrudBL(
            ITranslaterContextFactory contextFactory,
            IConfiguration configuration,
            IMapper mapper,
            ValidatorDto<InputKeywordByValueDtoBL, bool> validationKeywordByValue)
        {
            this.contextFactory = contextFactory;
            this.configuration = configuration;
            this.mapper = mapper;
            this.validationKeywordByValue = validationKeywordByValue;
        }

        public async Task<OutNewKeywordDtoBL> InsertNewKeywordAsync(
            string name,
            CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            await using var context = await contextFactory.CreateDbContext();

            if (await context.Set<Keyword>().AnyAsync(x => x.Value == name))
                throw new DtoVereficationException("name of keyword is existed yet, name must be unique");

            var languagesId = await context.Set<Language>().Select(x => x.Id).ToListAsync();

            var keyword = new Keyword
            {
                Value = name,
                Translations = languagesId
                    .Select(id => new Translation
                    {
                        LanguageId = id,
                        Value = string.Empty
                    }).ToList()
            };

            await context.Set<Keyword>().AddAsync(keyword);
            await context.SaveChangesAsync();

            var newKeyword = await context.Set<Keyword>()
                .AsNoTracking()
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Value == name);

            return mapper.Map<OutNewKeywordDtoBL>(newKeyword);
        }

        public async Task DeleteKeywordByValueAsync(
            string name,
            CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            await using var context = await contextFactory.CreateDbContext();

            var keyword = await context.Set<Keyword>().Select(x => new Keyword { Id = x.Id, Value = x.Value }).FirstOrDefaultAsync(x => x.Value == name);

            if (keyword is null)
                throw new DtoVereficationException("name of keyword does not exist in database");

            context.Set<Keyword>().Remove(keyword);
            await context.SaveChangesAsync();
        }

        public async Task<ICollection<OutGetKeywordByLanguagesDtoBL>> GetKeywordByValueAsync(
           InputKeywordByValueDtoBL input,
           CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            if (!await validationKeywordByValue.IsValid(input))
                throw validationKeywordByValue.Exception;

            await using var context = await contextFactory.CreateDbContext();


            if (int.TryParse(configuration["Common:AmountOutputdata"], out int count))
            {
                var result = await context.Set<Keyword>()
                    .AsNoTracking()
                    .Where(x => x.Translations.Any(t => input.LanguagesId.Contains(t.LanguageId)) &&
                          (string.IsNullOrEmpty(input.Value) || x.Value.ToLower().StartsWith(input.Value.ToLower())))
                    .Skip((input.Page - 1) * count)
                    .Take(count)
                    .Select(x => new Keyword
                    {
                        Id = x.Id,
                        Value = x.Value,
                        Translations = x.Translations
                                    .Where(t => input.LanguagesId.Contains(t.LanguageId))
                                    .Select(t => new Translation
                                    {
                                        Id = t.Id,
                                        LanguageId = t.LanguageId,
                                        Value = t.Value
                                    }).ToList()
                    })
                        .ToListAsync(token);

                if (result is null)
                    return new List<OutGetKeywordByLanguagesDtoBL>();


                return result.Select(x => mapper.Map<OutGetKeywordByLanguagesDtoBL>(x)).ToList();
            }

            throw new ConfigurationException("AmountOuputdata cannot specified in appsetting.json");
        }

        public async Task<int> GetCountPageAsync(
            CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            await using var context = await contextFactory.CreateDbContext();

            var countData = await context.Set<Keyword>().CountAsync(token);

            if (int.TryParse(configuration["Common:AmountOutputdata"], out int countOutputdata))
            {
                if ((countData % countOutputdata) >= 1)
                    return (countData / countOutputdata) + 1;

                return (countData / countOutputdata);
            }

            throw new ConfigurationException("AmountOuputdata cannot specified in appsetting.json");
        }
    }

    public interface IKeywordCrudBL
    {
        Task DeleteKeywordByValueAsync(string name, CancellationToken token);
        Task<int> GetCountPageAsync(CancellationToken token);
        Task<ICollection<OutGetKeywordByLanguagesDtoBL>> GetKeywordByValueAsync(InputKeywordByValueDtoBL input, CancellationToken token);
        Task<OutNewKeywordDtoBL> InsertNewKeywordAsync(string name, CancellationToken token);
    }
}
