namespace ExaminationSystem.Models
{
    public class StudentAnswer : BaseModel
    {
        public int StudentID { get; set; }

        public int ExamID { get; set; }

        public int QuestionID { get; set; }

        public int ChoiceID { get; set; }

        //Navigation Properties
        public Student Student { get; set; }

        public Exam Exam { get; set; }

        public Question Question { get; set; }

        public Choice Choice { get; set; }
    }
}
