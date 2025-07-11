using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TranslaterWebApi.ModelsUI.TranslationUI.Dto
{
    public class InputEditTranslationByIdDtoUI
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Include)]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "value", Required = Required.AllowNull, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Value { get; set; } = string.Empty;
    }
}
