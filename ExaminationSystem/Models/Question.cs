
using ExaminationSystem.Enums.Question;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    public class Question : BaseModel
    {
        public string Title { get; set; }

        public QuestionLevel Level { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }

        //navigation properties

        public ICollection<Choice> Choices { get; set; }

        public Course Course { get; set; }

        public Instructor Instructor { get; set; }

        public ICollection<ExamQuestion> ExamQuestions { get; set; }

        public ICollection<StudentAnswer> StudentAnswers { get; set; }
    }
}
