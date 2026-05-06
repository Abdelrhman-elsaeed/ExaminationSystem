using AutoMapper;
using ExaminationSystem.DTOs.User;
using ExaminationSystem.Models;
using ExaminationSystem.ViewModels.User;

namespace ExaminationSystem.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Ignore PasswordHash here since we hash it manually in the service
            CreateMap<AddUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());



            CreateMap<User, AddUserVM>();
            CreateMap<User, UpdateUserVM>();
            CreateMap<UserVM, UserDto>().ReverseMap();
        }
    }
}