using ExaminationSystem.BLL.DTOs.Exam;
using ExaminationSystem.BLL.DTOs.ExamQuestion;
using ExaminationSystem.BLL.DTOs.ExamStudent;
using ExaminationSystem.BLL.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface IExamService
    {
        Task<ResponseViewModel<ExamViewDTO>> AddAsync(CreateExamDTO model, CancellationToken cancellationToken = default);
        Task<bool> IsExist(int id, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<ExamViewDTO>> UpdateAsync(UpdateExamDTO model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> AssignQuestionToExam(AssignQuestionToExamDTO model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> UpdateQuestionOnExam(UpdateExamQuestionDTO model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> DeleteQuestoinFromExam(int examQuestionRecordId, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> AssignStudentToExam(CreateExamStudentDTO model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<ExamViewDTO>> ViewExam(int examId, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<decimal>> SubmitExam(SubmitExamDTO StudentAnswers, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> RandomExam(CreateRandomExamDTO model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<IEnumerable<ViewStudentsGradesDTO>>> ViewStudentsGrades(int ExamId, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<decimal?>> TopGrade(int ExamId, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<decimal?>> AverageGrade(int ExamId, CancellationToken cancellationToken = default);
    }
}
