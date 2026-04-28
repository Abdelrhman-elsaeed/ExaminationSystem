using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DTOs.ExamQuestion
{
    public class AssignQuestionToExamDTO
    {
        public int Grade { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
    }
}
