
namespace ExaminationSystem.BLL.ViewModels.Question
{
    public class CreateQuestionVM
    {
        public string Title { get; set; }
        public QuestionLevel Level { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public ICollection<CreateChoiceVM> Choices { get; set; }
    }
}
