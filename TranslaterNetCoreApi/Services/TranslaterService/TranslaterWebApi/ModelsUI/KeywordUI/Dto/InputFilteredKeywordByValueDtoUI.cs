﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TranslaterWebApi.ModelsUI.KeywordUI.Dto
{
    public class InputFilteredKeywordByValueDtoUI
    {
        [JsonProperty(PropertyName = "page", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Include)]
        [Range(1, int.MaxValue, ErrorMessage = "Page out of the acceptable range")]
        public int Page { get; set; } = 1;

        [JsonProperty(PropertyName = "filterValue", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Include)]
        public string FilterValue { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "languagesId", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Include)]
        public ICollection<Guid> LanguagesId { get; set; } = new List<Guid>();
    }
}
