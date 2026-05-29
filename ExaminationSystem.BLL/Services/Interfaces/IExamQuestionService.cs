using ExaminationSystem.BLL.DTOs.ExamQuestion;
using ExaminationSystem.BLL.DTOs.Question;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface IExamQuestionService
    {
        Task<bool> IsQuestionExistOnExam(int ExamId, int QuestionId, CancellationToken cancellationToken = default);
        Task<bool> AddAsync(AssignQuestionToExamDTO model, CancellationToken cancellationToken = default);
        Task<bool> AddRangeAsync(IEnumerable<AssignQuestionToExamDTO> models, CancellationToken cancellationToken = default);
        Task<bool> IsExist(int id, CancellationToken cancellationToken = default);
        Task<bool> DeleteQuestionFromExam(int id, CancellationToken cancellationToken = default);
        Task<ICollection<GetQuestionDTO>> GetExamQuestionsByExamId(int ExamId, CancellationToken cancellationToken = default);
        Task<ICollection<GetQuestionWithCorrectAnswerDTO>> GetExamQuestionsWithCorrectAnswersByExamId(int ExamId, CancellationToken cancellationToken = default);
        Task<bool> UpdateQuestionOnExam(UpdateExamQuestionDTO model, CancellationToken cancellationToken = default);
    }
}
