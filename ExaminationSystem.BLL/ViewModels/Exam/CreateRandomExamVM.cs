
namespace ExaminationSystem.BLL.ViewModels.Exam
{
    public class CreateRandomExamVM
    {
        public int CourseId { get; set; }
        public int ExamId { get; set; }
        public ICollection<RandomQuestionConfigVM> QuestionsConfig { get; set; }
    }
}
