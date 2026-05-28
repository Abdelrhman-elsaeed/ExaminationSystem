
namespace ExaminationSystem.BLL.AutoMapper.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<TokenRequestDto, TokenRequestVM>().ReverseMap();
            CreateMap<AddRoleDto, AddRoleVM>().ReverseMap();


            CreateMap<RegisterDto, User>().ReverseMap();
            CreateMap<RegisterDto, RegisterVM>().ReverseMap();


            CreateMap<AuthDto, AuthVM>().ReverseMap();

        }
    }
}
