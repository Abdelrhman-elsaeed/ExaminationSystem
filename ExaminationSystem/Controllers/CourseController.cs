using ExaminationSystem.DataBase;
using ExaminationSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IEnumerable<Course>> GetAll()
        {
            return await _Context.Courses
                .Where(x=>x.Deleted==false)
                .ToListAsync()
                .ConfigureAwait(true);
        }


        [HttpPost]
        public async Task<bool> Add(Course course)
        {
            _Context.Courses.Add(course);
            await _Context.SaveChangesAsync()
            .ConfigureAwait(true);

            return true;
        }


        [HttpGet]
        public async Task<Course> GetById(int id)
        {
            return await _Context.Courses.Where(x => x.Deleted == false && x.ID == id )
                .FirstOrDefaultAsync()
                .ConfigureAwait(true);
        }

        [HttpDelete]
        public async Task<bool> Delete(int id)
        {
            var course = await _Context.Courses
                .AsTracking()
                .FirstOrDefaultAsync(x=>x.ID==id);

            course.Deleted = true;

            await _Context.SaveChangesAsync();

            return true;

        }

    }
}
