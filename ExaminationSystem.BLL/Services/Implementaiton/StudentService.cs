using ExaminationSystem.DAL.Models;
using ExaminationSystem.BLL.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Implementaiton
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _StudentRepo;

        public StudentService(IRepository<Student> StudentRepo)
        {
            _StudentRepo = StudentRepo;
        }

        public async Task<bool> IsExistAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _StudentRepo.CheckExistsByConditionAsync(crs => crs.ID == id, cancellationToken);
        }
    }
}
