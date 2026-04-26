using ExaminationSystem.Models;
using ExaminationSystem.Repo;

namespace ExaminationSystem.Services
{
    public class InstructorService
    {
        private readonly GenericRepository<Instructor> _InstructorRepo;
        public InstructorService(GenericRepository<Instructor> InstructorRepo)
        {
            _InstructorRepo = InstructorRepo;
        }

        public async Task<bool> IsExist(int id)
        {
            return await _InstructorRepo.AnyAsync(crs => crs.ID == id);
        }
    }
}
