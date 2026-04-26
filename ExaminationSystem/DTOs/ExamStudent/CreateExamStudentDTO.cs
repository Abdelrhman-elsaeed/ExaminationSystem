using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DTOs.ExamStudent
{
    public class CreateExamStudentDTO
    {

        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public decimal? FinalGrade { get; set; }

    }
}
