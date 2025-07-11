using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthTrainingDL.Security
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;

        public JwtGenerator(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }

        public async Task<string> CreateToken(IdentityUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var claims = userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)).ToList();

            string secretKey = configuration["JWT:Secret"] ?? string.Empty;
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException($"{nameof(secretKey)} is null");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            string expiryTime = configuration["JWT:TokenExpiryMinutes"] ?? string.Empty;
            if (string.IsNullOrEmpty(expiryTime))
                throw new ArgumentNullException($"{nameof(expiryTime)} is null");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(int.Parse(expiryTime)),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

    public interface IJwtGenerator
    {
        Task<string> CreateToken(IdentityUser user);
    }
}
