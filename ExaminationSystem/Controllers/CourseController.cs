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

        private readonly GenericRepository<Course> _CourseRepo;

        public CourseController(GenericRepository<Course> CourseRepo)
        {
            _CourseRepo = CourseRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _CourseRepo.GetAll().ToListAsync());
        }

        [HttpGet]
        public async Task<Course> GetById(int id)
        {
            return await _CourseRepo.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<bool> Add(Course course)
        {
            return await _CourseRepo.AddAsync(course);
        }

        [HttpDelete]
        public async Task<bool> Delete(int id)
        {
            return await _CourseRepo.DeleteAsync(id);

        }

        [HttpPatch]
        public bool Update(UpdateCourseDTO model)
        {

            var newUpdates = new Course
            {
                ID = model.ID,
                Name = model.Name,
                Hours = model.Hours
            };

            _CourseRepo.UpdateInclude(newUpdates, nameof(Course.Name), nameof(Course.Hours));
            return true;
            
        }

    }
}
