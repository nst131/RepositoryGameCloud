using AutoMapper;
using TranslaterServiceBL.Common;
using TranslaterServiceBL.ModelsBL.TranslationBL.Dto;
using TranslaterServiceDL.Context;
using TranslaterServiceDL.Models;

namespace TranslaterServiceBL.ModelsBL.TranslationBL
{
    public class TranslationCrud : ITranslationCrud
    {
        private readonly ITranslaterContextFactory contextFactory;
        private readonly IMapper mapper;
        private readonly ValidatorDto<InputEditTranslationByIdDtoBL, bool> validatorEditTranslationByIdDto;

        public TranslationCrud(
            ITranslaterContextFactory contextFactory,
            IMapper mapper,
            ValidatorDto<InputEditTranslationByIdDtoBL, bool> validatorEditTranslationByIdDto)
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
            this.validatorEditTranslationByIdDto = validatorEditTranslationByIdDto;
        }

        public async Task EditTranslationValueByIdAsync(InputEditTranslationByIdDtoBL input, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            if (!await validatorEditTranslationByIdDto.IsValid(input))
                throw validatorEditTranslationByIdDto.Exception;

            await using var context = await contextFactory.CreateDbContext();

            var translation = new Translation() { Id = input.Id, Value = input.Value };

            context.Set<Translation>().Attach(translation);
            context.Entry(translation).Property(x => x.Value).IsModified = true;

            await context.SaveChangesAsync();
        }
    }

    public interface ITranslationCrud
    {
        Task EditTranslationValueByIdAsync(InputEditTranslationByIdDtoBL input, CancellationToken token);
    }
}
