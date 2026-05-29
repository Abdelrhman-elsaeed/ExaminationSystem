using ExaminationSystem.BLL.DTOs.User;
using ExaminationSystem.BLL.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsExistAsync(string id, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<UserDto>> AddAsync(AddUserDto model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<UserDto>> UpdateAsync(UpdateUserDto model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ResponseViewModel<UserDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
