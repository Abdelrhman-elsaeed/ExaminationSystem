
namespace ExaminationSystem.BLL.ViewModels.Exam
{
    public class ExamViewVM
    {
        public string Name { get; set; }

        public ExamType Type { get; set; }

        public DateTime Date { get; set; }

        public int DurationInMinutes { get; set; }

        public ICollection<GetQuestionVM> AllQuestion { get; set; }
    }
}
