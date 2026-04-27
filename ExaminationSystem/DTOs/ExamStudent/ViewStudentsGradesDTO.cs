using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DTOs.ExamStudent
{
    public class ViewStudentsGradesDTO
    {
        public int ID { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int ExamId { get; set; }
        public decimal? FinalGrade { get; set; }
    }
}
