using Microsoft.EntityFrameworkCore;
using TranslaterServiceBL.Common;
using TranslaterServiceBL.ModelsBL.TranslationBL.Dto;
using TranslaterServiceDL.Context;
using TranslaterServiceDL.Models;

namespace TranslaterServiceBL.ModelsBL.TranslationBL.Validation
{
    public class ValidEditTranslationByIdDtoBL(ITranslaterContextFactory factory) : ValidatorDto<InputEditTranslationByIdDtoBL, bool>
    {
        public async override Task<bool> IsValid(InputEditTranslationByIdDtoBL input)
        {
            if (input.Value is null)
            {
                Message = $"{input.Value} of translation is null";
                return false;
            }

            await using var context = await factory.CreateDbContext();

            if(!await context.Set<Translation>().AnyAsync(x => x.Id == input.Id))
            {
                Message = $"{input.Id} of Translation is not existed";
                return false;
            }

            return true;
        }
    }
}
