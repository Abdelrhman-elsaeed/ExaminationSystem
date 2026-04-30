using ExaminationSystem.DTOs.Question;
using ExaminationSystem.Enums.Exam;
using ExaminationSystem.ViewModels.Question;

namespace ExaminationSystem.ViewModels.Exam
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
