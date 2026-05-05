using AutoMapper;
using ExaminationSystem.DTOs.ExamQuestion;
using ExaminationSystem.DTOs.Question;
using ExaminationSystem.ModelDTO.Exam;
using ExaminationSystem.ModelDTO.ExamQuestion;
using ExaminationSystem.Models;
using ExaminationSystem.ViewModels.Exam;
using ExaminationSystem.ViewModels.ExamQuestion;

namespace ExaminationSystem.Helper.AutoMapper.Profiles
{
    public class ExamQuestionProfile : Profile
    {
        public ExamQuestionProfile()
        {
            CreateMap<AssignQuestionToExamDTO,AssignQuestionToExamVM>().ReverseMap();
            CreateMap<AssignQuestionToExamDTO,ExamQuestion>().ReverseMap();
            CreateMap<UpdateExamQuestionVM,UpdateExamQuestionDTO>().ReverseMap();
            CreateMap<UpdateExamQuestionDTO,ExamQuestion>().ReverseMap();
            CreateMap<GetQuestionDTO, ExamQuestion>().ReverseMap();
        }
    }
}
