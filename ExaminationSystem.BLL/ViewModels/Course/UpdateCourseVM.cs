using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExaminationSystem.BLL.ViewModels.Course
{
    public class UpdateCourseVM
    {
        [Required(ErrorMessage = "Course Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Course Name must be between 1 and 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Course Hours are required.")]
        public int Hours { get; set; }
    }
}
