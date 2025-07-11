using Microsoft.Extensions.DependencyInjection;
using TranslaterServiceBL.Common;
using TranslaterServiceBL.ModelsBL.KeywordBL;
using TranslaterServiceBL.ModelsBL.KeywordBL.Dto;
using TranslaterServiceBL.ModelsBL.KeywordBL.Validation;
using TranslaterServiceBL.ModelsBL.LanguageBL;
using TranslaterServiceBL.ModelsBL.TranslationBL;
using TranslaterServiceBL.ModelsBL.TranslationBL.Dto;
using TranslaterServiceBL.ModelsBL.TranslationBL.Validation;

namespace TranslaterServiceBL
{
    public static class ServiceRegistrationBL
    {
        public static void AddRegistrationBL(this IServiceCollection services)
        {
            //Language
            services.AddScoped<ILanguageCrudBL, LanguageCrudBL>();

            //Keyword
            services.AddScoped<IKeywordCrudBL, KeywordCrudBL>();
            services.AddScoped<ValidatorDto<InputKeywordByValueDtoBL, bool>, ValidKeywordByValue>();

            //Translation
            services.AddScoped<ITranslationCrud, TranslationCrud>();
            services.AddScoped<ValidatorDto<InputEditTranslationByIdDtoBL, bool>, ValidEditTranslationByIdDtoBL>();
        }
    }
}
