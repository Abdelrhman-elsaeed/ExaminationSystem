using ExaminationSystem.Enums.Question;
using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.ViewModels.Choice;

namespace ExaminationSystem.ViewModels.Question
{
    public class GetQuestionVM
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public QuestionLevel Level { get; set; }
        public int CourseId { get; set; }
        public ICollection<GetChoicesVM> Choices { get; set; }
    }
}
