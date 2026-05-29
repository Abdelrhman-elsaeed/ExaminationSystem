using ExaminationSystem.BLL.DTOs.ExamQuestion;
using ExaminationSystem.BLL.DTOs.Question;
using ExaminationSystem.BLL.DTOs.Choice;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.Services.Interfaces;
using ExaminationSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Implementaiton
{
    public class ExamQuestionService : IExamQuestionService
    {
        private readonly IRepository<ExamQuestion> _ExamQuestionRepo;
        public ExamQuestionService(IRepository<ExamQuestion> ExamQuestionRepo)
        {
            _ExamQuestionRepo = ExamQuestionRepo;
        }

        public async Task<bool> IsQuestionExistOnExam(int ExamId, int QuestionId, CancellationToken cancellationToken = default)
        {
            return await _ExamQuestionRepo.CheckExistsByConditionAsync(eq => eq.ExamId == ExamId && eq.QuestionId == QuestionId && eq.Deleted == false, cancellationToken);
        }

        public async Task<bool> AddAsync(AssignQuestionToExamDTO model, CancellationToken cancellationToken = default)
        {
            var AssignModel = model.Map<ExamQuestion>();
            await _ExamQuestionRepo.AddAsync(AssignModel, cancellationToken);
            return await _ExamQuestionRepo.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> AddRangeAsync(IEnumerable<AssignQuestionToExamDTO> models, CancellationToken cancellationToken = default)
        {
            var mappedModels = models?
                .Select(m => m.Map<ExamQuestion>())
                .ToList();

            if (mappedModels is null || mappedModels.Count == 0)
                return false;

            foreach (var m in mappedModels)
            {
                await _ExamQuestionRepo.AddAsync(m, cancellationToken);
            }
            return await _ExamQuestionRepo.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> IsExist(int id, CancellationToken cancellationToken = default)
        {
            return await _ExamQuestionRepo.CheckExistsByConditionAsync(q => q.ID == id && q.Deleted == false, cancellationToken);
        }

        public async Task<bool> DeleteQuestionFromExam(int id, CancellationToken cancellationToken = default)
        {
            var isExist = await this.IsExist(id, cancellationToken);
            if (!isExist)
                return false;

            var eq = await _ExamQuestionRepo.GetByIDAsync(id, cancellationToken);
            if (eq == null) return false;

            _ExamQuestionRepo.SoftDelete(eq);
            return await _ExamQuestionRepo.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<GetQuestionDTO>> GetExamQuestionsByExamId(int ExamId, CancellationToken cancellationToken = default)
        {
            if (ExamId <= 0)
                return new List<GetQuestionDTO>();

            return await _ExamQuestionRepo.GetByCondition(x => x.ExamId == ExamId && x.Deleted == false)
                .Where(eq => eq.Question != null && eq.Question.Deleted == false)
                .Select(eq => new GetQuestionDTO
                {
                    Title = eq.Question.Title,
                    Level = eq.Question.Level,
                    CourseId = eq.Question.CourseId,
                    Choices = eq.Question.Choices
                        .Where(c => c.Deleted == false)
                        .Select(c => new GetChoicesDTO
                        {
                            Text = c.Text,
                            ID = c.ID
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<ICollection<GetQuestionWithCorrectAnswerDTO>> GetExamQuestionsWithCorrectAnswersByExamId(int ExamId, CancellationToken cancellationToken = default)
        {
            if (ExamId <= 0)
                return new List<GetQuestionWithCorrectAnswerDTO>();

            return await _ExamQuestionRepo.GetByCondition(eq => eq.ExamId == ExamId && eq.Deleted == false)
                .Where(eq => eq.Question != null && eq.Question.Deleted == false)
                .Select(eq => new GetQuestionWithCorrectAnswerDTO()
                {
                    QuestionId = eq.QuestionId,
                    Grade = eq.Grade,
                    ChoiceId = eq.Question.Choices
                    .Where(c => c.Deleted == false && c.IsCorrectChoice == true)
                    .Select(c => c.ID)
                    .FirstOrDefault()
                }).ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateQuestionOnExam(UpdateExamQuestionDTO model, CancellationToken cancellationToken = default)
        {
            var IsExist = await this.IsExist(model.ID, cancellationToken);
            if (!IsExist)
                return false;

            var result = model.Map<ExamQuestion>();
            _ExamQuestionRepo.UpdateInclude(result, nameof(ExamQuestion.Grade));
            return await _ExamQuestionRepo.SaveChangesAsync(cancellationToken);
        }
    }
}
