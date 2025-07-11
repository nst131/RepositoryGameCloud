using AuthTraining.Models;
using AuthTrainingBL.Login;
using AuthTrainingBL.Login.Models;
using AuthTrainingBL.Registration;
using AuthTrainingBL.Registration.Models;
using AuthTrainingDL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthTraining.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginHandler loginHandler;
        private readonly IRegistrationHandler registrationHandler;
        private readonly IMapper mapper;

        public AuthController(
            ILoginHandler loginHandler,
            IRegistrationHandler registrationHandler,
            IMapper mapper)
        {
            this.loginHandler = loginHandler;
            this.registrationHandler = registrationHandler;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<OutputLoginDtoUI>> LoginAsync([FromBody] InputLoginDtoUI request)
        {
            if (request is null)
                throw new ArgumentNullException($"{nameof(request)} is null");

            var user = await loginHandler.Login(mapper.Map<InputLoginDto>(request));

            if (user is null)
                throw new UnauthorizedAccessException($"{nameof(user)} is null");

            var jwtToken = $"{JwtBearerDefaults.AuthenticationScheme}" + " " + $"{user.Token}";

            return new JsonResult(new OutputLoginDtoUI() { Token = jwtToken});
        }

        [AllowAnonymous]
        [HttpPost("RegistrationUser")]
        public async Task<ActionResult<string>> RegistrationUserAsync([FromBody] InputRegisrationForUserDtoUI request)
        {
            if (request is null)
                throw new ArgumentException($"{nameof(request)} is null");

            var user = await registrationHandler.Registration(mapper.Map<InputRegistrationDto>(request));

            if (user is null)
                throw new UnauthorizedAccessException($"{nameof(user)} is null");

            var jwtToken = $"{JwtBearerDefaults.AuthenticationScheme}" + " " + $"{user.Token}";

            return new JsonResult(new { Token = jwtToken, request.Email, Role = Role.User });
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("Registration")]
        public async Task<ActionResult<string>> RegistrationAsync([FromBody] InputRegistrationForAdminDtoUI request)
        {
            if (request is null)
                throw new ArgumentException($"{nameof(request)} is null");

            var user = await registrationHandler.Registration(mapper.Map<InputRegistrationDto>(request));

            if (user is null)
                throw new UnauthorizedAccessException($"{nameof(user)} is null");

            return new JsonResult(new { Response = "Registered" });
        }
    }
}
