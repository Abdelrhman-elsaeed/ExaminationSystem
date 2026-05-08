
namespace ExaminationSystem.BLL.AutoMapper.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<LoginRequestDto, LoginRequestVM>().ReverseMap();

        }
    }
}
