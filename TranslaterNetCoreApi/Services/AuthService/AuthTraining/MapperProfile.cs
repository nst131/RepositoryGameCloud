using AuthTraining.Models;
using AuthTrainingBL.Login.Models;
using AuthTrainingBL.Registration.Models;
using AutoMapper;

namespace AuthTraining
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<InputLoginDtoUI, InputLoginDto>();
            CreateMap<InputRegisrationForUserDtoUI, InputRegistrationDto>();
            CreateMap<InputRegistrationForAdminDtoUI, InputRegistrationDto>();
        }
    }
}
