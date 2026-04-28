using ExaminationSystem.Enums.Exam;

namespace ExaminationSystem.ViewModels.Exam
{
    public class CreateExamVM
    {
        public string Name { get; set; }

        public ExamType Type { get; set; }

        public DateTime Date { get; set; }

        public int DurationInMinutes { get; set; }

        public int InstructorId { get; set; }

        public int CourseId { get; set; }
    }
}
