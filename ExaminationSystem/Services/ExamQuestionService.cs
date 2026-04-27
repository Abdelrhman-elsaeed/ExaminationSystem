using ExaminationSystem.DTOs.Question;
using ExaminationSystem.Helper;
using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.ModelDTO.Exam;
using ExaminationSystem.Models;
using ExaminationSystem.Repo;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ExaminationSystem.Services
{
    public class ExamQuestionService
    {
        private readonly GenericRepository<ExamQuestion> _ExamQuestionRepo;
        public ExamQuestionService(GenericRepository<ExamQuestion> ExamQuestionRepo)
        {
            _ExamQuestionRepo = ExamQuestionRepo;
        }

        public async Task<bool> IsQuestionExistOnExam(int ExamId,int QuestionId)
        {
            return await _ExamQuestionRepo.AnyAsync(eq => eq.ExamId == ExamId && eq.QuestionId == QuestionId && eq.Deleted == false);
        }
        public async Task<bool> AddAsync(AssignQuestionToExamDTO model)
        {

            var AssignModel = model.Map<ExamQuestion>();

            return await _ExamQuestionRepo.AddAsync(AssignModel);
        }
        public async Task<bool> AddRangeAsync(IEnumerable<AssignQuestionToExamDTO> models)
        {
            var mappedModels = models?
                .Select(m => m.Map<ExamQuestion>())
                .ToList();

            if (mappedModels is null || mappedModels.Count == 0)
                return false;

            return await _ExamQuestionRepo.AddRangeAsync(mappedModels);
        }
        public async Task<bool> IsExist(int id)
        {
            return await _ExamQuestionRepo.AnyAsync(q => q.ID == id && q.Deleted==false);
        }
        public async Task<bool> DeleteQuestionFromExam(int id)
        {
            //1-validate ExamQuestion Record Exist
            var result = await this.IsExist(id);
            if (!result)
                return false;

            return await _ExamQuestionRepo.DeleteAsync(id);
        }
        public async Task<ICollection<GetQuestionDTO>> GetExamQuestionsByExamId(int ExamId)
        {
            if (ExamId <= 0)
                return new List<GetQuestionDTO>();

            return await _ExamQuestionRepo.Get(x => x.ExamId == ExamId && x.Deleted == false)
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
                .ToListAsync();
        }
        public async Task<ICollection<GetQuestionWithCorrectAnswerDTO>> GetExamQuestionsWithCorrectAnswersByExamId(int ExamId)
        {
            if (ExamId <= 0)
                return new List<GetQuestionWithCorrectAnswerDTO>();

            return await _ExamQuestionRepo.Get(eq => eq.ExamId == ExamId && eq.Deleted == false)
                .Where(eq => eq.Question != null && eq.Question.Deleted == false)
                .Select(eq => new GetQuestionWithCorrectAnswerDTO()
                {
                    QuestionId = eq.QuestionId,
                    Grade=eq.Grade,
                    ChoiceId = eq.Question.Choices
                    .Where(c => c.Deleted == false && c.IsCorrectChoice == true)
                    .Select(c => c.ID)
                    .FirstOrDefault()

                }).ToListAsync();
        }


    }
}
