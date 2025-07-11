using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AuthTraining.Models
{
    public class InputRegisrationForUserDtoUI
    {
        [JsonProperty(PropertyName = "email", Required = Required.Always)]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "password", Required = Required.Always)]
        //[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        //ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; } = string.Empty;
    }
}
