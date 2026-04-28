using ExaminationSystem.DTOs.Choice;
using ExaminationSystem.Enums.Question;
using ExaminationSystem.ModelDTO.Choice;

namespace ExaminationSystem.ModelDTO.Question
{
    public class GetAllQuestionDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public QuestionLevel Level { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public ICollection<GetAllChoicesDTO> Choices { get; set; }
    }
}
