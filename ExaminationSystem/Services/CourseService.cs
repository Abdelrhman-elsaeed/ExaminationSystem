using ExaminationSystem.Models;
using ExaminationSystem.Repo;

namespace ExaminationSystem.Services
{
    public class CourseService
    {
        private readonly GenericRepository<Course> _CourseRepo;
        public CourseService(GenericRepository<Course> CourseRepo)
        {
            _CourseRepo = CourseRepo;
        }

        public async Task<bool> IsExist(int id)
        {
            return await _CourseRepo.AnyAsync(crs => crs.ID == id);
        }
    }
}
