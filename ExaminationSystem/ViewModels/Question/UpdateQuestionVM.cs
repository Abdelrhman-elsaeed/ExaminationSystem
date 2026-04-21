using ExaminationSystem.Enums.Question;

namespace ExaminationSystem.ViewModels.Question
{
    public class UpdateQuestionVM
    {

        public int ID { get; set; }
        public QuestionLevel Level { get; set; }
        public string Title { get; set; }
    }
}
