using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TranslaterWebApi.ModelsUI.KeywordUI.Dto
{
    public class InputInsertNewKeywordDtoUI
    {
        [JsonProperty(PropertyName = "name", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Include)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Length of the string can be from 3 to 50 symbols")]
        public string Name { get; set; } = string.Empty;
    }
}
