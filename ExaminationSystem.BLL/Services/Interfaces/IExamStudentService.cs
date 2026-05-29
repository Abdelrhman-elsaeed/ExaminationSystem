using ExaminationSystem.BLL.DTOs.ExamStudent;
using ExaminationSystem.BLL.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface IExamStudentService
    {
        Task<bool> AddAsync(CreateExamStudentDTO model, CancellationToken cancellationToken = default);
        Task<bool> SaveFinalGrade(int examId, int studentId, decimal finalGrade, CancellationToken cancellationToken = default);
        Task<bool> IsStudentAssignedToExamAsync(int examId, int studentId, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<decimal?>> StudentFinalGrade(int StudentId, int ExamId, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<IEnumerable<ViewStudentsGradesDTO>>> ViewStudentsGrades(int ExamId, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<decimal?>> TopGrade(int ExamId, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<decimal?>> AverageGrade(int ExamId, CancellationToken cancellationToken = default);
    }
}
