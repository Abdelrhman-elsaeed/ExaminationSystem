using ExaminationSystem.BLL.DTOs.StudentCourse;
using ExaminationSystem.BLL.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface IStudnetCourseService
    {
        Task<bool> IsExistAsync(int StudentId, int CourseId, CancellationToken cancellationToken = default);
        Task<bool> IsExistByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> AssignStudentToCourseAsync(AssignStudentToCourseDTO model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> DeleteStudentFromCourseAsync(int id, CancellationToken cancellationToken = default);
    }
}
