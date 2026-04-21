using ExaminationSystem.DataBase;
using ExaminationSystem.DTOs.Question;
using ExaminationSystem.Helper;
using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.ModelDTO.Question;
using ExaminationSystem.Models;
using ExaminationSystem.ModelVm.Course;
using ExaminationSystem.ModelVm.Question;
using ExaminationSystem.Repo;
using ExaminationSystem.ViewModels.Question;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ExaminationSystem.Services
{
    public class QuestionService
    {

        private readonly GenericRepository<Question> _QuestionRepo;
        private readonly ChoiceService _ChoiceService;
        public QuestionService(GenericRepository<Question> QuestionRepo, ChoiceService ChoiceService)
        {
            _QuestionRepo = QuestionRepo;
            _ChoiceService = ChoiceService;
        }
        public async Task<bool> AddAsync(CreateQuestionDTO model)
        {
            //Mapping (DTO to Model)

            var newQuestion = model.Map<Question>();

            //Call Question Repo 
            var result = await _QuestionRepo.AddAsync(newQuestion);

            return result;
        }

        public async Task<GetQuestionDTO?> GetAsync(int id)
        {
            //Question validation
            // The result will be null if the question wasn't found, 
            // which can easily be checked in Controller to return a 404 Not Found.

            var question = await _QuestionRepo.Get(x => x.ID == id && x.Deleted == false)
                .FirstOrDefaultAsync();
            return question.Map<GetQuestionDTO>();
        }

        public async Task<IEnumerable<GetAllQuestionDTO>> GetAllAsync()
        {
            var AllQuestions = await _QuestionRepo.GetAll()
                .Project<GetAllQuestionDTO>()
                .ToListAsync();

            return AllQuestions;
        }

        public async Task<bool> DeleteQuestionAndChoicesAsync(int id)
        {
            // TransactionScope keeps DB logic out of the service layer
            using var DeleteTransaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                // Delete Question
                var QuestionResult = await _QuestionRepo.DeleteAsync(id);
                if (!QuestionResult) return false;

                //Delete Question Choices
                var ChoicesResult = await _ChoiceService.DeleteByQuestionIdAsync(id);

                //commit Transaction
                DeleteTransaction.Complete();
                return true;

            }
            catch
            {
                // Automatically rolls back if Complete() is not called
                throw;

            }
        }

        public async Task<bool> UpdateQuestionAsync(UpdateQuestionDTO model)
        {
            //validate question exist 
            var IsQuestionExist = await _QuestionRepo.AnyAsync(x => x.ID == model.ID);
            if (!IsQuestionExist) return false;

            //mapping 
            var NewUpdates = model.Map<Question>();
            
            //call repo
            _QuestionRepo.UpdateInclude(NewUpdates, nameof(Question.Title),nameof(Question.Level));

            // return result 
            return true;
        }

    }
}
