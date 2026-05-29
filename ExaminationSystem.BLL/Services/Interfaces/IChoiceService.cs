using ExaminationSystem.BLL.DTOs.Choice;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface IChoiceService
    {
        Task<bool> DeleteByQuestionIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> UpdateChoiceAsync(UpdateChoiceDTO model, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(int id, CancellationToken cancellationToken = default);
    }
}
