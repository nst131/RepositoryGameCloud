using AuthTrainingBL.Login.Models;
using AuthTrainingDL.Security;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace AuthTrainingBL.Login
{
    public class LoginHandler : ILoginHandler
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IJwtGenerator jwtGenerator;

        public LoginHandler(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IJwtGenerator jwtGenerator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtGenerator = jwtGenerator;
        }

        public async Task<ResponseLoginDto> Login(InputLoginDto request)
        {
            if (string.IsNullOrEmpty(request.Email))
                throw new UnauthorizedAccessException($"{request.Email} is empty");

            if (string.IsNullOrEmpty(request.Password))
                throw new UnauthorizedAccessException($"{request.Password} is empty");

            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                throw new UnauthorizedAccessException($"{nameof(user)} is not exist");

            var passwordVerification = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

            if (!passwordVerification.Succeeded)
                throw new UnauthorizedAccessException($"{nameof(user)} is not pass verification of password");

            var userRoles = await userManager.GetRolesAsync(user);

            if (userRoles.Any())
            {
                return new ResponseLoginDto
                {
                    Token = await jwtGenerator.CreateToken(user),
                    Email = request.Email,
                    Role = userRoles.FirstOrDefault() ?? "Undefined"
                };
            }

            throw new UnauthorizedAccessException($"{nameof(user)} is not pass verification of role");
        }
    }

    public interface ILoginHandler
    {
        Task<ResponseLoginDto> Login(InputLoginDto request);
    }
}
