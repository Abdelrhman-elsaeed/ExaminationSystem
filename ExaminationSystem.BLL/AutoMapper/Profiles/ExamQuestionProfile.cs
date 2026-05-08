
namespace ExaminationSystem.BLL.AutoMapper.Profiles
{
    public class ExamQuestionProfile : Profile
    {
        public ExamQuestionProfile()
        {
            CreateMap<AssignQuestionToExamDTO, AssignQuestionToExamVM>().ReverseMap();
            CreateMap<AssignQuestionToExamDTO, ExamQuestion>().ReverseMap();
            CreateMap<UpdateExamQuestionVM, UpdateExamQuestionDTO>().ReverseMap();
            CreateMap<UpdateExamQuestionDTO, ExamQuestion>().ReverseMap();
            CreateMap<GetQuestionDTO, ExamQuestion>().ReverseMap();
        }
    }
}
