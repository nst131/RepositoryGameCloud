using Newtonsoft.Json;

namespace TranslaterWebApi.ModelsUI.LanguageUI.Dto
{
    public class OutLanguageDtoUI
    {
        [JsonProperty(PropertyName = "id", Order = 0,
            NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include)]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name", Order = 1,
            NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Name { get; set; } = string.Empty;
    }
}
