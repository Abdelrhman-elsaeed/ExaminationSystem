using AutoMapper;
using ExaminationSystem.DTOs.Auth;
using ExaminationSystem.DTOs.Choice;
using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.ViewModels.Auth;
using ExaminationSystem.ViewModels.Choice;

namespace ExaminationSystem.Helper.AutoMapper.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<LoginRequestDto, LoginRequestVM>().ReverseMap();
            
        }
    }
}
