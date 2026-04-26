using ExaminationSystem.DTOs.Student;
using ExaminationSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DTOs.Exam
{
    public class SubmitExamDTO
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }

        public List<StudentAnswerDTO> Answers { get; set; }
    }
}
