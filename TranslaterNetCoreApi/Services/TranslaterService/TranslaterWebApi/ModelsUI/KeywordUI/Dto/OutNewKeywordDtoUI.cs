using Newtonsoft.Json;
using TranslaterServiceBL.ModelsBL.KeywordBL.Dto;

namespace TranslaterWebApi.ModelsUI.KeywordUI.Dto
{
    public class OutNewKeywordDtoUI
    {
        [JsonProperty(PropertyName = "id", Order = 0,
            NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include)]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "value", Order = 1,
            NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Value { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "outTranslationWithLanguages", Order = 2,
            NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include)]
        public ICollection<OutTranslationWithLanguageDtoBL> OutTranslationWithLanguages { get; set; } = new List<OutTranslationWithLanguageDtoBL>();
    }
}
