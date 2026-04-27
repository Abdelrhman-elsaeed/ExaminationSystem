using ExaminationSystem.Enums.Question;

namespace ExaminationSystem.DTOs.Question
{
    public class GetQuestionRelatedToCourseDTO
    {
        public int ID { get; set; }
        public QuestionLevel Level { get; set; }
    }
}
