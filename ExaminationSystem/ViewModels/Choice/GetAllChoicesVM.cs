using ExaminationSystem.Enums.Question;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.ViewModels.Choice
{
    public class GetAllChoicesVM
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public int QuestionId { get; set; }
    }
}
