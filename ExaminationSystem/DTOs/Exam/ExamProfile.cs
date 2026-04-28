using AutoMapper;
using ExaminationSystem.ModelDTO.Exam;
using ExaminationSystem.Models;
using ExaminationSystem.ModelVm.Question;
using ExaminationSystem.ViewModels.Exam;

namespace ExaminationSystem.DTOs.Exam
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            CreateMap<CreateExamDTO, CreateExamVM>().ReverseMap();
            CreateMap<CreateExamDTO, ExaminationSystem.Models.Exam>().ReverseMap();
            CreateMap<UpdateExamDTO, UpdateExamVM>().ReverseMap();
            CreateMap<UpdateExamDTO, ExaminationSystem.Models.Exam>().ReverseMap();
        }
    }
}
