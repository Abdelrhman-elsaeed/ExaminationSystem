

using ExaminationSystem.BLL.Helper.BusinessExceptions;

namespace ExaminationSystem.BLL.Services
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
        public async Task<ResponseViewModel<bool>> AddAsync(CreateQuestionDTO model)
        {
            //Mapping (DTO to Model)

            var newQuestion = model.Map<Question>();

            //Call Question Repo 
            var result = await _QuestionRepo.AddAsync(newQuestion);

            if (result)
                return ResponseViewModel<bool>.Success(result, ErrorCode.None, message: "Question Added Successfully");
            else
                return ResponseViewModel<bool>.Failure(ErrorCode.QuestionAddFail, message: "Failed to add the question. Please try again");
        }

        public async Task<ResponseViewModel<GetQuestionDTO>> GetAsync(int id)
        {

            var question = await _QuestionRepo.Get(x => x.ID == id && x.Deleted == false)
                .FirstOrDefaultAsync();
            var result = question.Map<GetQuestionDTO>();

            if (result != null)
                return ResponseViewModel<GetQuestionDTO>.Success(result, ErrorCode.None, message: "Question retrieved successfully");
            else
                return ResponseViewModel<GetQuestionDTO>.Failure(ErrorCode.QustionNotFound, message: "Question not found");

        }

        public async Task<ResponseViewModel<IEnumerable<GetAllQuestionDTO>>> GetAllAsync()
        {
            var AllQuestions = await _QuestionRepo.GetAll()
                .Project<GetAllQuestionDTO>()
                .ToListAsync();

            if (AllQuestions.Count > 0)
                return ResponseViewModel<IEnumerable<GetAllQuestionDTO>>.Success(AllQuestions, ErrorCode.None, message: "Questions retrieved successfully");
            else
                return ResponseViewModel<IEnumerable<GetAllQuestionDTO>>.Failure(ErrorCode.QustionNotFound, message: "No questions found");
        }

        public async Task<ResponseViewModel<bool>> DeleteQuestionAndChoicesAsync(int id)
        {
            // Delete Question
            var QuestionResult = await _QuestionRepo.DeleteAsync(id);
            if (!QuestionResult)
            {
                // Throw an exception so the transaction rolls back and the global error handler takes over
                throw new BusinessException(ErrorCode.QuestionDeleteFail, "Faild to delete Question");
            }

            // Delete Question Choices
            var ChoicesResult = await _ChoiceService.DeleteByQuestionIdAsync(id);
            if (!ChoicesResult)
            {
                throw new BusinessException(ErrorCode.ChoiceDeleteFail, "Faild to delete Chocices");
            }

            return ResponseViewModel<bool>.Success(true, message: "Question and choices Deleted Successfully");
        }

        public async Task<ResponseViewModel<bool>> UpdateQuestionAsync(UpdateQuestionDTO model)
        {
            //validate question exist 
            var IsQuestionExist = await _QuestionRepo.AnyAsync(x => x.ID == model.ID && x.Deleted == false);
            if (!IsQuestionExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.QustionNotFound, message: "Failed to find the question");

            //mapping 
            var NewUpdates = model.Map<Question>();

            //call repo
            var result = await _QuestionRepo.UpdateInclude(NewUpdates, nameof(Question.Title), nameof(Question.Level));

            // return result 
            if (result)
                return ResponseViewModel<bool>.Success(result, ErrorCode.None, message: "Question Updated Successfully");
            else
                return ResponseViewModel<bool>.Failure(ErrorCode.QuestionUpdateFail, message: "Question failed to Update");

        }

        public async Task<ResponseViewModel<bool>> UpdateChoiceAsync(UpdateChoiceDTO model)
        {
            //validate Choice exist 
            var IsChoiceExist = await _ChoiceService.AnyAsync(model.ID);
            if (!IsChoiceExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ChoiceNotFound, message: "Failed to find the choice");


            var result = await _ChoiceService.UpdateChoiceAsync(model);

            // return result 
            if (result)
                return ResponseViewModel<bool>.Success(result, ErrorCode.None, message: "Choice Updated Successfully");
            else
                return ResponseViewModel<bool>.Failure(ErrorCode.ChoiceUpdateFail, message: "Choice failed to Update");

        }

        public async Task<bool> IsExistAsync(int id)
        {
            return await _QuestionRepo.AnyAsync(q => q.ID == id && q.Deleted == false);
        }

        public async Task<IEnumerable<GetQuestionRelatedToCourseDTO>> GetQuestionsByCourseId(int CourseId)
        {
            return await _QuestionRepo.Get(q => q.CourseId == CourseId && !q.Deleted)
               .Project<GetQuestionRelatedToCourseDTO>()
               .ToListAsync();
        }

        public async Task<List<int>> GetRandomQuestionIdsByCourseAndLevelAsync(int courseId, QuestionLevel level, int count, ICollection<int>? excludedQuestionIds = null)
        {
            //excludedQuestionIds this list we will (store ids that we have before) to prevent Duplications.

            if (courseId <= 0 || count <= 0)
                return new List<int>();

            //initial query
            var query = _QuestionRepo.Get(q =>
                q.CourseId == courseId &&
                !q.Deleted &&
                q.Level == level);


            if (excludedQuestionIds is not null && excludedQuestionIds.Count > 0)
            {
                query = query.Where(q => !excludedQuestionIds.Contains(q.ID));
            }

            //final query
            return await query
                .OrderBy(q => Guid.NewGuid())
                .Select(q => q.ID)
                .Take(count)
                .ToListAsync();
        }


    }
}
