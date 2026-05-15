using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.BLL.ViewModels.Course
{
    public class GetCourseVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Hours { get; set; }
    }
}
