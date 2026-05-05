using AutoMapper;
using ExaminationSystem.DTOs.Exam;
using ExaminationSystem.ModelDTO.Exam;
using ExaminationSystem.Models;
using ExaminationSystem.ModelVm.Question;
using ExaminationSystem.ViewModels.Exam;

namespace ExaminationSystem.Helper.AutoMapper.Profiles
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            CreateMap<CreateExamDTO, CreateExamVM>().ReverseMap();
            CreateMap<CreateExamDTO, Exam>().ReverseMap();
            CreateMap<UpdateExamDTO, UpdateExamVM>().ReverseMap();
            CreateMap<UpdateExamDTO, Exam>().ReverseMap();
            CreateMap<ExamViewDTO, ExamViewVM>().ReverseMap();
            CreateMap<ExamViewDTO, Exam>().ReverseMap();

            CreateMap<CreateRandomExamDTO, CreateRandomExamVM>().ReverseMap();
            CreateMap<CreateRandomExamDTO, Exam>().ReverseMap();



        }
    }
}
