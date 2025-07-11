using AuthTrainingBL.Registration.Models;
using AuthTrainingDL.Security;
using Microsoft.AspNetCore.Identity;

namespace AuthTrainingBL.Registration
{
    public class RegistrationHandler : IRegistrationHandler
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IJwtGenerator jwtGenerator;

        public RegistrationHandler(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtGenerator jwtGenerator)
        {
            this.userManager = userManager;
            this.jwtGenerator = jwtGenerator;
            this.roleManager = roleManager;
        }

        public async Task<ResponseRegistrationDto> Registration(InputRegistrationDto request)
        {
            if (string.IsNullOrEmpty(request.Email))
                throw new NullReferenceException($"{nameof(request.Email)} is empty");

            if (string.IsNullOrEmpty(request.Password))
                throw new NullReferenceException($"{nameof(request.Password)} is empty");

            if (string.IsNullOrEmpty(request.Name))
                throw new NullReferenceException($"{nameof(request.Name)} is empty");

            if (string.IsNullOrEmpty(request.Role))
                throw new NullReferenceException($"{nameof(request.Role)} is empty");

            if (!await roleManager.RoleExistsAsync(request.Role))
                throw new ArgumentException($"{nameof(request.Role)} does not exist");

            var user = new IdentityUser { UserName = request.Name, Email = request.Email };

            var resultAppendUser = await userManager.CreateAsync(user, request.Password);

            if (!resultAppendUser.Succeeded)
                throw new ArgumentException(resultAppendUser.Errors.FirstOrDefault()?.Description);

            var resultAppendRole = await userManager.AddToRoleAsync(user, request.Role);

            if (resultAppendRole.Succeeded)
                return new ResponseRegistrationDto()
                {
                    Token = await jwtGenerator.CreateToken(user),
                    Email = request.Email,
                    Role = request.Role
                };

            throw new ArgumentException(resultAppendRole.Errors.FirstOrDefault()?.Description);
        }
    }

    public interface IRegistrationHandler
    {
        Task<ResponseRegistrationDto> Registration(InputRegistrationDto request);
    }
}
