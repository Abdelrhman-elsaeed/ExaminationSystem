using ExaminationSystem.Enums.Question;
using ExaminationSystem.ModelDTO.Choice;

namespace ExaminationSystem.DTOs.Question
{
    public class GetQuestionWithCorrectAnswerDTO
    {

        public int QuestionId { get; set; }
        public int ChoiceId { get; set; }
        public int Grade { get; set; }

    }
}
