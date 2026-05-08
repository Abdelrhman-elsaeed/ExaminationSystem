

namespace ExaminationSystem.DAL.Models
{
    public class ExamStudent : BaseModel
    {

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [ForeignKey("Exam")]
        public int ExamId { get; set; }
        public decimal? FinalGrade { get; set; }
        public Student Student { get; set; }
        public Exam Exam { get; set; }
    }
}
