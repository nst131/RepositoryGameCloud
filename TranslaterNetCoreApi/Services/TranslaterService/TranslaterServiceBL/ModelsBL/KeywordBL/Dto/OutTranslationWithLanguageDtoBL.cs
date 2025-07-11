using Newtonsoft.Json;

namespace TranslaterServiceBL.ModelsBL.KeywordBL.Dto
{
    public class OutTranslationWithLanguageDtoBL
    {
        [JsonProperty(PropertyName = "languageId", Order = 0,
            NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include)]
        public Guid LanguageId { get; set; }

        [JsonProperty(PropertyName = "translationId", Order = 1,
            NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include)]
        public Guid TranslationId { get; set; }

        [JsonProperty(PropertyName = "translationValue", Order = 2,
            NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include)]
        public string TranslationValue { get; set; } = string.Empty;    
    }
}
