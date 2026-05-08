
namespace ExaminationSystem.BLL.DTOs.Question
{
    public class CreateQuestionDTO
    {
        public string Title { get; set; }
        public QuestionLevel Level { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public ICollection<CreateChoiceDTO> Choices { get; set; }
    }
}
