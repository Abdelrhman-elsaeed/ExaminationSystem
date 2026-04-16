using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.ModelDTO.ExamQuestion
{
    public class UpdateExamQuestionDTO
    {
        public int ID { get; set; }
        public int Grade { get; set; }
    }
}
