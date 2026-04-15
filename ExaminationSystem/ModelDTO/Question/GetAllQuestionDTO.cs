using ExaminationSystem.ModelDTO.Choice;

namespace ExaminationSystem.ModelDTO.Question
{
    public class GetAllQuestionDTO
    {
        public int id { get; set; }
        public string Title { get; set; }
        public ICollection<GetAllChoicesDTO> Choices { get; set; }
    }
}
