using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DTOs.Student
{
    public class StudentAnswerDTO
    {
        public int ChoiceId { get; set; }
        public int QuestionId { get; set; }
    }
}
