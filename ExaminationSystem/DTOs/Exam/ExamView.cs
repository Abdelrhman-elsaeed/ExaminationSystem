using ExaminationSystem.Enums.Exam;
using ExaminationSystem.ModelDTO.Question;
using ExaminationSystem.Models;

namespace ExaminationSystem.DTOs.Exam
{
    public class ExamView
    {
        public string Name { get; set; }

        public ExamType Type { get; set; }

        public DateTime Date { get; set; }

        public int DurationInMinutes { get; set; }

        public ICollection<ExaminationSystem.Models.Question> AllQuestion { get; set; } 
    }
}
 