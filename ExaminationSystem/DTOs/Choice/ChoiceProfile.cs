using AutoMapper;
using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.ModelDTO.Question;
using ExaminationSystem.ViewModels.Choice;
using ExaminationSystem.ViewModels.Question;

namespace ExaminationSystem.DTOs.Choice
{
    public class ChoiceProfile : Profile
    {

        public ChoiceProfile()
        {
            CreateMap<CreateChoiceVM, CreateChoiceDTO>().ReverseMap();
            CreateMap<CreateChoiceDTO, ExaminationSystem.Models.Choice>().ReverseMap();

            CreateMap<GetAllChoicesDTO, ExaminationSystem.Models.Choice>().ReverseMap();
            CreateMap<GetAllChoicesVM, GetAllChoicesDTO>().ReverseMap();
            
            CreateMap<ExaminationSystem.Models.Choice, GetChoicesDTO>();

            CreateMap<GetChoicesDTO, ExaminationSystem.Models.Choice>().ReverseMap();
            
        }
    }
}
