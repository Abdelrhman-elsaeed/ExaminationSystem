using ExaminationSystem.Enums.Question;

namespace ExaminationSystem.DTOs.Question
{
    public class RandomQuestionConfigDTO
    {
        public int Count { get; set; } 
        public QuestionLevel Level { get; set; } 
        public int GradePerQuestion { get; set; } 
    }
}
