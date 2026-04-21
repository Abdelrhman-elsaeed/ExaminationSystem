using ExaminationSystem.Enums.Question;
using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DTOs.Question
{
    public class GetQuestionDTO
    {
        public string Title { get; set; }
        public QuestionLevel Level { get; set; }
        public int CourseId { get; set; }
        public ICollection<GetChoicesDTO> Choices { get; set; }
    }
}
