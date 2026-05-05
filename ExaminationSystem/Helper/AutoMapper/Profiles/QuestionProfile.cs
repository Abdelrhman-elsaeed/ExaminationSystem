using AutoMapper;
using ExaminationSystem.DTOs.Question;
using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.ModelDTO.Question;
using ExaminationSystem.Models;
using ExaminationSystem.ModelVm.Question;
using ExaminationSystem.ViewModels.Choice;
using ExaminationSystem.ViewModels.Question;

namespace ExaminationSystem.Helper.AutoMapper.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            // i used ExaminationSystem.Models.Question instead of Question Because my Folder Name is Question so there was confilict 
            CreateMap<CreateQuestionDTO, Question>().ReverseMap();

            CreateMap<GetQuestionDTO, Question>().ReverseMap();
            CreateMap<GetQuestionDTO, GetQuestionVM>().ReverseMap();

            CreateMap<UpdateQuestionDTO, Question>().ReverseMap();

            CreateMap<CreateQuestionVM, CreateQuestionDTO>().ReverseMap();

            CreateMap<GetAllQuestionVM, GetAllQuestionDTO>().ReverseMap();
            CreateMap<GetAllQuestionDTO, Question>().ReverseMap();

            CreateMap<UpdateQuestionVM, UpdateQuestionDTO>().ReverseMap();


            
            CreateMap<Question, GetAllQuestionDTO>();
                
        }
    }
}
