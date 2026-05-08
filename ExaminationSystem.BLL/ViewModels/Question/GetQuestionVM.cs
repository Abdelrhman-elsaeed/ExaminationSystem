
namespace ExaminationSystem.BLL.ViewModels.Question
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
