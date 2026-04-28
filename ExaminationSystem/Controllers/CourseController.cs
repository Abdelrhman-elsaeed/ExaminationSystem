using ExaminationSystem.DataBase;
using ExaminationSystem.Models;
using ExaminationSystem.ModelVm.Course;
using ExaminationSystem.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class CourseController : ControllerBase
    {

        //private readonly GenericRepository<Course> _CourseRepo;

        //public CourseController(GenericRepository<Course> CourseRepo)
        //{
        //    _CourseRepo = CourseRepo;
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
            
        //}

        //[HttpGet]
        //public async Task<Course> GetById(int id)
        //{
            
        //}

        //[HttpPost]
        //public async Task<bool> Add(Course course)
        //{
            
        //}

        //[HttpDelete]
        //public async Task<bool> Delete(int id)
        //{
            

        //}

        //[HttpPatch]
        //public bool Update(UpdateCourseDTO model)
        //{
            
        //}

    }
}
