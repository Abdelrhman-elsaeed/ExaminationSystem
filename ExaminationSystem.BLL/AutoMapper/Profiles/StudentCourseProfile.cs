using ExaminationSystem.BLL.DTOs.StudentCourse;
using ExaminationSystem.BLL.ViewModels.StudentCourse;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.BLL.AutoMapper.Profiles
{
    public class StudentCourseProfile:Profile
    {
        public StudentCourseProfile()
        {
            CreateMap<AssignStudentToCourseDTO, StudentCourse>().ReverseMap();
            CreateMap<AssignStudentToCourseDTO, AssignStudentToCourseVM>().ReverseMap();
        }
    }
}
