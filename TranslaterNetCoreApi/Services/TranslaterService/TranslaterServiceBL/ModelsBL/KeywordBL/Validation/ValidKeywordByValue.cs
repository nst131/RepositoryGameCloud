using Microsoft.EntityFrameworkCore;
using TranslaterServiceBL.Common;
using TranslaterServiceBL.ModelsBL.KeywordBL.Dto;
using TranslaterServiceDL.Context;
using TranslaterServiceDL.Models;

namespace TranslaterServiceBL.ModelsBL.KeywordBL.Validation
{
    public class ValidKeywordByValue(ITranslaterContextFactory factory) : ValidatorDto<InputKeywordByValueDtoBL, bool>
    {
        public async override Task<bool> IsValid(InputKeywordByValueDtoBL input)
        {
            if (input.Page <= 0)
            {
                Message = $"{input.Page} of pagination out of the range of acceptable range";
                return false;
            }

            if(input.Value is null)
            {
                Message = $"{input.Value} of Translation is null";
                return false;
            }

            await using var context = await factory.CreateDbContext();

            if (input.LanguagesId.Any())
            {
                var languagesFromDatabase = await context.Set<Language>()
                 .Where(x => input.LanguagesId.Contains(x.Id))
                 .Select(x => x.Id)
                 .ToListAsync();

                if (!input.LanguagesId.Count.Equals(languagesFromDatabase.Count))
                {
                    Message = $"One of the {nameof(Language)} does not exist";
                    return false;
                }
            }
            else
            {
                Message = $"Collection {nameof(Language)} with id is empty, but must contain at least one language id";
                return false;
            }

            return true;
        }
    }
}
