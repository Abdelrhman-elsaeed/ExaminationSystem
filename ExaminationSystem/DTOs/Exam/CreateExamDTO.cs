using ExaminationSystem.Enums.Exam;

namespace ExaminationSystem.ModelDTO.Exam
{
    public class CreateExamDTO
    {
        public string Name { get; set; }

        public ExamType Type { get; set; }

        public DateTime Date { get; set; }

        public int DurationInMinutes { get; set; }

        public int InstructorId { get; set; }

        public int CourseId { get; set; }
    }
}
