using ExaminationSystem.Enums.Question;

namespace ExaminationSystem.ViewModels.Question
{
    public class RandomQuestionConfigVM
    {
        public int Count { get; set; }
        public QuestionLevel Level { get; set; }
        public int GradePerQuestion { get; set; }
    }
}
