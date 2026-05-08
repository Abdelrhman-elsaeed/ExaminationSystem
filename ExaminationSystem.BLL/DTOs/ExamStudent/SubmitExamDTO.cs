
namespace ExaminationSystem.BLL.DTOs.ExamStudent
{
    public class SubmitExamDTO
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }

        public List<StudentAnswerDTO> Answers { get; set; }
    }
}
