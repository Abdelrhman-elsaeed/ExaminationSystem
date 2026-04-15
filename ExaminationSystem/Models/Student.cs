namespace ExaminationSystem.Models
{
    public class Student : BaseModel
    {

        public string Name { get; set; }

        public ICollection<ExamStudent> ExamStudents { get; set; }

        public ICollection<StudentAnswer> StudentAnswers { get; set; }
    }
}
