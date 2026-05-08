
namespace ExaminationSystem.BLL.DTOs.Question
{
    public class GetQuestionDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public QuestionLevel Level { get; set; }
        public int CourseId { get; set; }
        public ICollection<GetChoicesDTO> Choices { get; set; }
    }
}
