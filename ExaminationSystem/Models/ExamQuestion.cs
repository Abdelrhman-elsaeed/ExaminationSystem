using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    public class ExamQuestion : BaseModel
    {
        public int Grade { get; set; }

        [ForeignKey("Exam")]
        public int ExamId { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        //Navigation Properties

        public Exam Exam { get; set; }
        public Question Question { get; set; }
    }
}
