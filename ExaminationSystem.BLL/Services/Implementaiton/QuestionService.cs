using ExaminationSystem.BLL.DTOs.Choice;
using ExaminationSystem.BLL.DTOs.Common;
using ExaminationSystem.BLL.DTOs.Question;
using ExaminationSystem.BLL.Helper.BusinessExceptions;
using ExaminationSystem.BLL.ViewModels;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.BLL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Implementaiton
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _QuestionRepo;
        private readonly IChoiceService _ChoiceService;
        
        public QuestionService(IRepository<Question> QuestionRepo, IChoiceService ChoiceService)
        {
            _QuestionRepo = QuestionRepo;
            _ChoiceService = ChoiceService;
        }
        
        public async Task<ResponseViewModel<GetQuestionDTO>> AddAsync(CreateQuestionDTO model, CancellationToken cancellationToken = default)
        {
            var newQuestion = model.Map<Question>();
            await _QuestionRepo.AddAsync(newQuestion, cancellationToken);
            var result = await _QuestionRepo.SaveChangesAsync(cancellationToken);

            if (result)
                return ResponseViewModel<GetQuestionDTO>.Success(newQuestion.Map<GetQuestionDTO>(), ErrorCode.None, message: "Question Added Successfully");
            else
                return ResponseViewModel<GetQuestionDTO>.Failure(ErrorCode.SaveQuestionFail, message: "Failed to save the question. Please try again");
        }

        public async Task<ResponseViewModel<GetQuestionDTO>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var question = await _QuestionRepo.GetByCondition(x => x.ID == id && x.Deleted == false)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (question == null)
                return ResponseViewModel<GetQuestionDTO>.Failure(ErrorCode.QustionNotFound, message: "Question not found");

            var result = question.Map<GetQuestionDTO>();

            return ResponseViewModel<GetQuestionDTO>.Success(result, ErrorCode.None, message: "Question retrieved successfully");
        }

        public async Task<ResponseViewModel<PagedResult<GetAllQuestionDTO>>> GetAllAsync(PaginationParams parameters, CancellationToken cancellationToken = default)
        {
            var query = _QuestionRepo.GetAll();

            // Filtering
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                query = query.Where(q => q.Title.Contains(parameters.SearchTerm));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                if (parameters.OrderBy.Equals("title", StringComparison.OrdinalIgnoreCase))
                    query = parameters.IsDescending ? query.OrderByDescending(q => q.Title) : query.OrderBy(q => q.Title);
                else if (parameters.OrderBy.Equals("level", StringComparison.OrdinalIgnoreCase))
                    query = parameters.IsDescending ? query.OrderByDescending(q => q.Level) : query.OrderBy(q => q.Level);
                else
                    query = query.OrderBy(q => q.ID);
            }
            else
            {
                query = query.OrderBy(q => q.ID);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var questions = await query.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                       .Take(parameters.PageSize)
                                       .Project<GetAllQuestionDTO>()
                                       .ToListAsync(cancellationToken);

            var pagedResult = new PagedResult<GetAllQuestionDTO>
            {
                Items = questions,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            return ResponseViewModel<PagedResult<GetAllQuestionDTO>>.Success(pagedResult, ErrorCode.None, message: "Questions retrieved successfully");
        }

        public async Task<ResponseViewModel<bool>> DeleteQuestionAndChoicesAsync(int id, CancellationToken cancellationToken = default)
        {
            var question = await _QuestionRepo.GetByIDAsync(id, cancellationToken);
            if (question == null)
                throw new BusinessException(ErrorCode.QuestionDeleteFail, "Failed to delete Question");

            _QuestionRepo.SoftDelete(question);
            var QuestionResult = await _QuestionRepo.SaveChangesAsync(cancellationToken);
            if (!QuestionResult)
            {
                throw new BusinessException(ErrorCode.SaveQuestionFail, "Failed to save the deleted Question state");
            }

            var ChoicesResult = await _ChoiceService.DeleteByQuestionIdAsync(id, cancellationToken);
            if (!ChoicesResult)
            {
                throw new BusinessException(ErrorCode.ChoiceDeleteFail, "Failed to delete Choices");
            }

            return ResponseViewModel<bool>.Success(true, message: "Question and choices Deleted Successfully");
        }

        public async Task<ResponseViewModel<GetQuestionDTO>> UpdateQuestionAsync(UpdateQuestionDTO model, CancellationToken cancellationToken = default)
        {
            var IsQuestionExist = await _QuestionRepo.CheckExistsByConditionAsync(x => x.ID == model.ID && x.Deleted == false, cancellationToken);
            if (!IsQuestionExist)
                return ResponseViewModel<GetQuestionDTO>.Failure(ErrorCode.QustionNotFound, message: "Failed to find the question");

            var NewUpdates = model.Map<Question>();

            _QuestionRepo.UpdateInclude(NewUpdates, nameof(Question.Title), nameof(Question.Level));
            var result = await _QuestionRepo.SaveChangesAsync(cancellationToken);

            if (result)
                return ResponseViewModel<GetQuestionDTO>.Success(NewUpdates.Map<GetQuestionDTO>(), ErrorCode.None, message: "Question Updated Successfully");
            else
                return ResponseViewModel<GetQuestionDTO>.Failure(ErrorCode.SaveQuestionFail, message: "Question failed to save updates");
        }

        public async Task<ResponseViewModel<bool>> UpdateChoiceAsync(UpdateChoiceDTO model, CancellationToken cancellationToken = default)
        {
            var IsChoiceExist = await _ChoiceService.AnyAsync(model.ID, cancellationToken);
            if (!IsChoiceExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ChoiceNotFound, message: "Failed to find the choice");

            var result = await _ChoiceService.UpdateChoiceAsync(model, cancellationToken);

            if (result)
                return ResponseViewModel<bool>.Success(result, ErrorCode.None, message: "Choice Updated Successfully");
            else
                return ResponseViewModel<bool>.Failure(ErrorCode.ChoiceUpdateFail, message: "Choice failed to Update");
        }

        public async Task<bool> IsExistAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _QuestionRepo.CheckExistsByConditionAsync(q => q.ID == id && q.Deleted == false, cancellationToken);
        }

        public async Task<IEnumerable<GetQuestionRelatedToCourseDTO>> GetQuestionsByCourseId(int CourseId, CancellationToken cancellationToken = default)
        {
            return await _QuestionRepo.GetByCondition(q => q.CourseId == CourseId && !q.Deleted)
               .Project<GetQuestionRelatedToCourseDTO>()
               .ToListAsync(cancellationToken);
        }

        public async Task<List<int>> GetRandomQuestionIdsByCourseAndLevelAsync(int courseId, QuestionLevel level, int count, ICollection<int>? excludedQuestionIds = null, CancellationToken cancellationToken = default)
        {
            if (courseId <= 0 || count <= 0)
                return new List<int>();

            var query = _QuestionRepo.GetByCondition(q =>
                q.CourseId == courseId &&
                !q.Deleted &&
                q.Level == level);

            if (excludedQuestionIds is not null && excludedQuestionIds.Count > 0)
            {
                query = query.Where(q => !excludedQuestionIds.Contains(q.ID));
            }

            return await query
                .OrderBy(q => Guid.NewGuid())
                .Select(q => q.ID)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}
