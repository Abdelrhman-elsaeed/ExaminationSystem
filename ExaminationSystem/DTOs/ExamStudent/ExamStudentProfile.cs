using AutoMapper;
using ExaminationSystem.DTOs.Student;
using ExaminationSystem.ModelDTO.Exam;
using ExaminationSystem.Models;
using ExaminationSystem.ViewModels.Exam;
using ExaminationSystem.ViewModels.ExamStudent;

namespace ExaminationSystem.DTOs.ExamStudent
{
    public class ExamStudentProfile : Profile
    {
        public ExamStudentProfile()
        {
            CreateMap<SubmitExamDTO, SubmitExamVM>().ReverseMap();
            CreateMap<SubmitExamDTO,  ExaminationSystem.Models.ExamStudent> ().ReverseMap();
            CreateMap<StudentAnswerVM, StudentAnswerDTO> ().ReverseMap();
            CreateMap<StudentAnswerDTO,StudentAnswer> ().ReverseMap();
            CreateMap<ViewStudentsGradesDTO, ViewStudentsGradesVM> ().ReverseMap();
            CreateMap<ViewStudentsGradesDTO, ExaminationSystem.Models.ExamStudent> ().ReverseMap();
            CreateMap<CreateExamStudentDTO, CreateExamStudentVM> ().ReverseMap();
            CreateMap<CreateExamStudentDTO, ExaminationSystem.Models.ExamStudent> ().ReverseMap();
        }
    }
}
