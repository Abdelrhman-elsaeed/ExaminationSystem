
namespace ExaminationSystem.BLL.DTOs.Exam
{
    public class ExamView
    {
        public string Name { get; set; }

        public ExamType Type { get; set; }

        public DateTime Date { get; set; }

        public int DurationInMinutes { get; set; }

        public ICollection<DAL.Models.Question> AllQuestion { get; set; }
    }
}
