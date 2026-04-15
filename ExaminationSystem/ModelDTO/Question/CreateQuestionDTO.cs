using ExaminationSystem.Enums.Question;
using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.Models;

namespace ExaminationSystem.ModelVm.Question
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
