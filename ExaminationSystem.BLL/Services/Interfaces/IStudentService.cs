using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface IStudentService
    {
        Task<bool> IsExistAsync(int id, CancellationToken cancellationToken = default);
    }
}
