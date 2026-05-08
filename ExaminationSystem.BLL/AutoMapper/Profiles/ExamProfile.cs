
namespace ExaminationSystem.BLL.AutoMapper.Profiles
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
