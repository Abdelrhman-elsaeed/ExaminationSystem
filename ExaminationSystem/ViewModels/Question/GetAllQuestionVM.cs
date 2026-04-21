using ExaminationSystem.Enums.Question;
using ExaminationSystem.ViewModels.Choice;

namespace ExaminationSystem.ViewModels.Question
{
    public class GetAllQuestionVM
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public QuestionLevel Level { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public ICollection<GetAllChoicesVM> Choices { get; set; }
    }
}
