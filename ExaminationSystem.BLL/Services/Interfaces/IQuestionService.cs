using ExaminationSystem.BLL.DTOs.Question;
using ExaminationSystem.BLL.DTOs.Common;
using ExaminationSystem.BLL.DTOs.Choice;
using ExaminationSystem.BLL.ViewModels;
using ExaminationSystem.DAL.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<ResponseViewModel<GetQuestionDTO>> AddAsync(CreateQuestionDTO model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<GetQuestionDTO>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<PagedResult<GetAllQuestionDTO>>> GetAllAsync(PaginationParams parameters, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> DeleteQuestionAndChoicesAsync(int id, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<GetQuestionDTO>> UpdateQuestionAsync(UpdateQuestionDTO model, CancellationToken cancellationToken = default);
        Task<ResponseViewModel<bool>> UpdateChoiceAsync(UpdateChoiceDTO model, CancellationToken cancellationToken = default);
        Task<bool> IsExistAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<GetQuestionRelatedToCourseDTO>> GetQuestionsByCourseId(int CourseId, CancellationToken cancellationToken = default);
        Task<List<int>> GetRandomQuestionIdsByCourseAndLevelAsync(int courseId, QuestionLevel level, int count, ICollection<int>? excludedQuestionIds = null, CancellationToken cancellationToken = default);
    }
}
