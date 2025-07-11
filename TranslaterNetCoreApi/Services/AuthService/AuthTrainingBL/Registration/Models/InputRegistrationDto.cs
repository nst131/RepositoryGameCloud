namespace AuthTrainingBL.Registration.Models
{
    public class InputRegistrationDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = AuthTrainingDL.Models.Role.User;
    }
}
