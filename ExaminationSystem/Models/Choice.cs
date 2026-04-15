using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    public class Choice : BaseModel
    {
        public string Text { get; set; }

        public bool IsCorrectChoice { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }


        //navigation properties
        public Question Question { get;set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; }
    }
}
