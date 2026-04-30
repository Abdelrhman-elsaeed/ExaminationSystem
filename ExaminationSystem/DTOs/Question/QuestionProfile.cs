using AutoMapper;
using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.ModelDTO.Question;
using ExaminationSystem.Models;
using ExaminationSystem.ModelVm.Question;
using ExaminationSystem.ViewModels.Choice;
using ExaminationSystem.ViewModels.Question;

namespace ExaminationSystem.DTOs.Question
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            // i used ExaminationSystem.Models.Question instead of Question Because my Folder Name is Question so there was confilict 
            CreateMap<CreateQuestionDTO, ExaminationSystem.Models.Question>().ReverseMap();

            CreateMap<GetQuestionDTO, ExaminationSystem.Models.Question>().ReverseMap();
            CreateMap<GetQuestionDTO, GetQuestionVM>().ReverseMap();

            CreateMap<UpdateQuestionDTO, ExaminationSystem.Models.Question>().ReverseMap();

            CreateMap<CreateQuestionVM, CreateQuestionDTO>().ReverseMap();

            CreateMap<GetAllQuestionVM, GetAllQuestionDTO>().ReverseMap();
            CreateMap<GetAllQuestionDTO, ExaminationSystem.Models.Question>().ReverseMap();

            CreateMap<UpdateQuestionVM, UpdateQuestionDTO>().ReverseMap();


            
            CreateMap<ExaminationSystem.Models.Question, GetAllQuestionDTO>();
                
        }
    }
}
