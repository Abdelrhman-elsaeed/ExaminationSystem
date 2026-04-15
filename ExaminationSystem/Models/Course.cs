namespace ExaminationSystem.Models
{
    public class Course : BaseModel
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public int Hours { get; set; }

        //navigation properties

        public ICollection<Question> Questions { get; set; }
        public ICollection<Exam> Exams { get; set; }
    }
}
