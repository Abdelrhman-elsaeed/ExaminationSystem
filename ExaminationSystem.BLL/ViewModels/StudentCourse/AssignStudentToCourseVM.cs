using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExaminationSystem.BLL.ViewModels.StudentCourse
{
    public class AssignStudentToCourseVM
    {
        [Required(ErrorMessage = "Student ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Student ID must be a positive number")]
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Course ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Course ID must be a positive number")]
        public int CourseID { get; set; }
    }
}
