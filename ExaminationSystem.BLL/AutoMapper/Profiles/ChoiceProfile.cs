
namespace ExaminationSystem.BLL.AutoMapper.Profiles
{
    public class ChoiceProfile : Profile
    {

        public ChoiceProfile()
        {
            CreateMap<CreateChoiceVM, CreateChoiceDTO>().ReverseMap();
            CreateMap<CreateChoiceDTO, Choice>().ReverseMap();

            CreateMap<GetAllChoicesDTO, Choice>().ReverseMap();
            CreateMap<GetAllChoicesVM, GetAllChoicesDTO>().ReverseMap();
            CreateMap<GetChoicesDTO, GetChoicesVM>().ReverseMap();
            CreateMap<GetChoicesDTO, Choice>().ReverseMap();

            CreateMap<Choice, GetChoicesDTO>();
            CreateMap<UpdateChoiceDTO, Choice>().ReverseMap();
            CreateMap<UpdateChoiceVM, UpdateChoiceDTO>().ReverseMap();



            CreateMap<GetChoicesDTO, Choice>().ReverseMap();

        }
    }
}
