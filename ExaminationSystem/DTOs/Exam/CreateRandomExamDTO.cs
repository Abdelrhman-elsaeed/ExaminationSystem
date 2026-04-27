using ExaminationSystem.DTOs.Question;

namespace ExaminationSystem.DTOs.Exam
{
    public class CreateRandomExamDTO
    {
        public int CourseId { get; set; }
        public int ExamId { get; set; }
        public ICollection<RandomQuestionConfigDTO> QuestionsConfig { get; set; }
    }
}
