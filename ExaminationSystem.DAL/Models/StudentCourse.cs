using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.DAL.Models
{
    public class StudentCourse : BaseModel
    {
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("Student")]
        public int StudentID { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public Course Course { get; set; }
    }
}
