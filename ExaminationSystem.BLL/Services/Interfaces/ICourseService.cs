using ExaminationSystem.BLL.DTOs.Course;
using ExaminationSystem.BLL.DTOs.Common;
using ExaminationSystem.BLL.ViewModels;
using ExaminationSystem.BLL.ViewModels.Course;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface ICourseService
    {
        Task<bool> IsExist(int id, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<GetCourseDTO>> AddCourseAsync(CreateCourseDTO model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<GetCourseDTO>> GetCourseByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<PagedResult<GetCourseDTO>>> GetAllCoursesAsync(PaginationParams parameters, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<GetCourseDTO>> UpdateCourseAsync(UpdateCourseDTO model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
