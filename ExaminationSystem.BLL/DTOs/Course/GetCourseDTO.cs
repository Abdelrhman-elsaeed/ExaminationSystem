using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.BLL.DTOs.Course
{
    public class GetCourseDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Hours { get; set; }
    }
}
