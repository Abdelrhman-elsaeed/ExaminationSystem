using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.ModelDTO.Exam
{
    public class AssignQuestionToExamDTO
    {
        public int Grade { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
    }
}
