
namespace ExaminationSystem.BLL.ViewModels.Exam
{
    public class UpdateExamVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ExamType Type { get; set; }
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
