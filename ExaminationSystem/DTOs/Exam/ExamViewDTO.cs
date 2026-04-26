using ExaminationSystem.DTOs.Question;
using ExaminationSystem.Enums.Exam;

namespace ExaminationSystem.DTOs.Exam
{
    public class ExamViewDTO
    {
        public string Name { get; set; }

        public ExamType Type { get; set; }

        public DateTime Date { get; set; }

        public int DurationInMinutes { get; set; }

        public ICollection<GetQuestionDTO> AllQuestion { get; set; }
    }
}
