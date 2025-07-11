using Newtonsoft.Json;

namespace TranslaterWebApi.ModelsUI.KeywordUI.Dto
{
    public class InputDeleteKeywordByValueDtoUI
    {
        [JsonProperty(PropertyName = "name", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Name { get; set; } = string.Empty;
    }
}
