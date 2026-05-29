using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface IInstructorService
    {
        Task<bool> IsExist(int id, CancellationToken cancellationToken = default);
    }
}
