using ExaminationSystem.DAL.Models;
using ExaminationSystem.BLL.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Implementaiton
{
    public class InstructorService : IInstructorService
    {
        private readonly IRepository<Instructor> _InstructorRepo;
        public InstructorService(IRepository<Instructor> InstructorRepo)
        {
            _InstructorRepo = InstructorRepo;
        }

        public async Task<bool> IsExist(int id, CancellationToken cancellationToken = default)
        {
            return await _InstructorRepo.CheckExistsByConditionAsync(crs => crs.ID == id, cancellationToken);
        }
    }
}
