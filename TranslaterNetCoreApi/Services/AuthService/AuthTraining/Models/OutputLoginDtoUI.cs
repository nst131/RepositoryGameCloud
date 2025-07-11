using Newtonsoft.Json;

namespace AuthTraining.Models
{
    public class OutputLoginDtoUI
    {
        [JsonProperty(PropertyName = "token", Order = 0,
          NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Token { get; set; } = string.Empty;
    }
}
