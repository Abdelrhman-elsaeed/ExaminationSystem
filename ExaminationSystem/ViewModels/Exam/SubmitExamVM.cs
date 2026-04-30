using ExaminationSystem.DTOs.Student;

namespace ExaminationSystem.ViewModels.Exam
{
    public class SubmitExamVM
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }

        public List<StudentAnswerVM> Answers { get; set; }
    }
}
