using ExaminationSystem.BLL.DTOs.Course;
using ExaminationSystem.BLL.ViewModels.Course;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.BLL.AutoMapper.Profiles
{
    public class CourseProfile:Profile
    {
        public CourseProfile()
        {
            CreateMap<CreateCourseDTO, Course>().ReverseMap();
            CreateMap<CreateCourseDTO, CreateCourseVM>().ReverseMap();

            CreateMap<GetCourseDTO, Course>().ReverseMap();
            CreateMap<GetCourseDTO, GetCourseVM>().ReverseMap();

            CreateMap<UpdateCourseDTO, Course>().ReverseMap();
            CreateMap<UpdateCourseDTO, UpdateCourseVM>().ReverseMap();
        }
    }
}
