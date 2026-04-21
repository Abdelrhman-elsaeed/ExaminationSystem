using ExaminationSystem.DTOs.Choice;
using ExaminationSystem.ModelDTO.Choice;

namespace ExaminationSystem.ModelDTO.Question
{
    public class GetAllQuestionDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public ICollection<GetAllChoicesDTO> Choices { get; set; }
    }
}
