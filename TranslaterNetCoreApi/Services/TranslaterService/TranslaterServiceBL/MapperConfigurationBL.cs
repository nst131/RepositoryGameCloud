using AutoMapper;
using TranslaterServiceBL.ModelsBL.KeywordBL.Dto;
using TranslaterServiceBL.ModelsBL.LanguageBL.Dto;
using TranslaterServiceDL.Models;

namespace TranslaterServiceBL
{
    public class MapperConfigurationBL : Profile
    {
        public MapperConfigurationBL()
        {
            //Language
            CreateMap<Language, OutLaguageDtoBL>();

            //Keyword
            CreateMap<Keyword, OutGetKeywordByLanguagesDtoBL>().ConvertUsing((keyword, outKeyword) => new OutGetKeywordByLanguagesDtoBL()
            {
                Id = keyword.Id,
                Value = keyword.Value,
                OutTranslationWithLanguages = keyword.Translations.Select(x => new OutTranslationWithLanguageDtoBL
                {
                    TranslationId = x.Id,
                    LanguageId = x.LanguageId,
                    TranslationValue = x.Value
                }).ToList() ?? new List<OutTranslationWithLanguageDtoBL>()
            });
            CreateMap<Keyword, OutNewKeywordDtoBL>().ConvertUsing((keyword, outKeyword) => new OutNewKeywordDtoBL()
            {
                Id = keyword.Id,
                Value = keyword.Value,
                OutTranslationWithLanguages = keyword.Translations.Select(x => new OutTranslationWithLanguageDtoBL()
                {
                    LanguageId = x.LanguageId,
                    TranslationId = x.Id,
                    TranslationValue = x.Value
                }).ToList()
            });
        }
    }
}
