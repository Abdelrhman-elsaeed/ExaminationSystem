namespace ExaminationSystem.ViewModels.ExamStudent
{
    public class ViewStudentsGradesVM
    {
        public int ID { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int ExamId { get; set; }
        public decimal? FinalGrade { get; set; }
    }
}
