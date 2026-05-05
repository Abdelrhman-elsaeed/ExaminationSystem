using AutoMapper;
using ExaminationSystem.DTOs.Choice;
using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.ModelDTO.Question;
using ExaminationSystem.ViewModels.Choice;
using ExaminationSystem.ViewModels.Question;

namespace ExaminationSystem.Helper.AutoMapper.Profiles
{
    public class ChoiceProfile : Profile
    {

        public ChoiceProfile()
        {
            CreateMap<CreateChoiceVM, CreateChoiceDTO>().ReverseMap();
            CreateMap<CreateChoiceDTO, Models.Choice>().ReverseMap();

            CreateMap<GetAllChoicesDTO, Models.Choice>().ReverseMap();
            CreateMap<GetAllChoicesVM, GetAllChoicesDTO>().ReverseMap();
            CreateMap<GetChoicesDTO, GetChoicesVM>().ReverseMap();
            CreateMap<GetChoicesDTO, Models.Choice>().ReverseMap();

            CreateMap<Models.Choice, GetChoicesDTO>();
            CreateMap<UpdateChoiceDTO, Models.Choice>().ReverseMap();
            CreateMap<UpdateChoiceVM, UpdateChoiceDTO>().ReverseMap();



            CreateMap<GetChoicesDTO, Models.Choice>().ReverseMap();
            
        }
    }
}
