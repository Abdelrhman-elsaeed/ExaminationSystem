
namespace ExaminationSystem.BLL.AutoMapper.Profiles
{
    public class ExamStudentProfile : Profile
    {
        public ExamStudentProfile()
        {
            CreateMap<SubmitExamDTO, SubmitExamVM>().ReverseMap();
            CreateMap<SubmitExamDTO, ExamStudent>().ReverseMap();
            CreateMap<StudentAnswerVM, StudentAnswerDTO>().ReverseMap();
            CreateMap<StudentAnswerDTO, StudentAnswer>().ReverseMap();
            CreateMap<ViewStudentsGradesDTO, ViewStudentsGradesVM>().ReverseMap();
            CreateMap<ViewStudentsGradesDTO, ExamStudent>().ReverseMap();
            CreateMap<CreateExamStudentDTO, CreateExamStudentVM>().ReverseMap();
            CreateMap<CreateExamStudentDTO, ExamStudent>().ReverseMap();
        }
    }
}
