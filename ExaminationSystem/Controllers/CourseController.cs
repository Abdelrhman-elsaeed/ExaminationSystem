using ExaminationSystem.DataBase;
using ExaminationSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class CourseController : ControllerBase
    {

        private readonly Context _Context;

        public CourseController()
        {
            _Context = new Context();
        }

        [HttpGet]
        public IEnumerable<Course> GetAll()
        {
            return _Context.Courses.ToList();

        }


        [HttpPost]
        public bool Add(Course course)
        {
            _Context.Courses.Add(course);
            _Context.SaveChanges();

            return true;
        }




    }
}
