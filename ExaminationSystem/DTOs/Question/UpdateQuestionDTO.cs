using ExaminationSystem.Enums.Question;
using ExaminationSystem.ModelDTO.Course;
using ExaminationSystem.Models;

namespace ExaminationSystem.ModelVm.Question
{
    public class UpdateQuestionDTO
    {
        public int ID { get; set; }
        public QuestionLevel Level { get; set; }
        public string Title { get; set; }
    }
}
