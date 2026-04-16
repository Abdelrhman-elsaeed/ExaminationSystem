using ExaminationSystem.Enums.Exam;

namespace ExaminationSystem.ModelDTO.Exam
{
    public class UpdateExamDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ExamType Type { get; set; }
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
