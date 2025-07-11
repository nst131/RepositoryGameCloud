using AutoMapper;
using TranslaterServiceBL.ModelsBL.KeywordBL.Dto;
using TranslaterServiceBL.ModelsBL.LanguageBL.Dto;
using TranslaterServiceBL.ModelsBL.TranslationBL.Dto;
using TranslaterWebApi.ModelsUI.KeywordUI.Dto;
using TranslaterWebApi.ModelsUI.LanguageUI.Dto;
using TranslaterWebApi.ModelsUI.TranslationUI.Dto;

namespace TranslaterWebApi
{
    public class MapperConfigurationUI : Profile
    {
        public MapperConfigurationUI()
        {
            //Language
            CreateMap<OutLaguageDtoBL, OutLanguageDtoUI>();
            CreateMap<InputInsertNewLanguageDtoUI, InputInsertNewLanguageDtoBL>();

            //Keyword
            CreateMap<OutGetKeywordByLanguagesDtoBL, OutKeywordByLanguagesDtoUI>();
            CreateMap<InputKeywordByLanguagesDtoUI, InputKeywordByValueDtoBL>();
            CreateMap<OutNewKeywordDtoBL, OutNewKeywordDtoUI>();
            CreateMap<InputEditTranslationByIdDtoUI, InputEditTranslationByIdDtoBL>();
            CreateMap<InputKeywordByValueDtoUI, InputKeywordByValueDtoBL>();
            CreateMap<InputFilteredKeywordByValueDtoUI, InputKeywordByValueDtoBL>();
        }
    }
}
