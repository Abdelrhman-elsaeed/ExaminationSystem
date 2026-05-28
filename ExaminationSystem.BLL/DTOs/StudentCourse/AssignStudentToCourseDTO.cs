using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ExaminationSystem.BLL.DTOs.StudentCourse
{
    public class AssignStudentToCourseDTO
    {
        public int StudentID { get; set; }
        public int CourseID { get; set; }
    }
}
