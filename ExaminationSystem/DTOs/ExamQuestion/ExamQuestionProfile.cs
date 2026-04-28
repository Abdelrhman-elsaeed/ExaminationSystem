using AutoMapper;
using ExaminationSystem.ModelDTO.Exam;
using ExaminationSystem.ModelDTO.ExamQuestion;
using ExaminationSystem.Models;
using ExaminationSystem.ViewModels.Exam;
using ExaminationSystem.ViewModels.ExamQuestion;

namespace ExaminationSystem.DTOs.ExamQuestion
{
    public class ExamQuestionProfile : Profile
    {
        public ExamQuestionProfile()
        {
            CreateMap<AssignQuestionToExamDTO,AssignQuestionToExamVM>().ReverseMap();
            CreateMap<AssignQuestionToExamDTO,ExaminationSystem.Models.ExamQuestion>().ReverseMap();
            CreateMap<UpdateExamQuestionVM,UpdateExamQuestionDTO>().ReverseMap();
            CreateMap<UpdateExamQuestionDTO,ExaminationSystem.Models.ExamQuestion>().ReverseMap();
        }
    }
}
