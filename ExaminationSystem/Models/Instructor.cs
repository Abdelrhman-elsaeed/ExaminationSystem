namespace ExaminationSystem.Models
{
    public class Instructor : BaseModel
    {
        public string Name { get; set; }
        public ICollection<Exam> Exams { get; set; }
    }
}
