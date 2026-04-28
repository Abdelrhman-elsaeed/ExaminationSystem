using ExaminationSystem.DTOs.Exam;
using ExaminationSystem.DTOs.ExamQuestion;
using ExaminationSystem.DTOs.ExamStudent;
using ExaminationSystem.DTOs.Question;
using ExaminationSystem.Enums;
using ExaminationSystem.Enums.Exam;
using ExaminationSystem.Helper;
using ExaminationSystem.ModelDTO.Exam;
using ExaminationSystem.ModelDTO.ExamQuestion;
using ExaminationSystem.Models;
using ExaminationSystem.ModelVm.Question;
using ExaminationSystem.Repo;
using ExaminationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Transactions;

namespace ExaminationSystem.Services
{
    public class ExamService
    {

        private readonly GenericRepository<Exam> _ExamRepo;
        private readonly CourseService _CourseService;
        private readonly InstructorService _InstructorService;
        private readonly ExamQuestionService _ExamQuestionService;
        private readonly QuestionService _QuestionService;
        private readonly ExamStudentService _ExamStudentService;


        public ExamService(GenericRepository<Exam> ExamRepo, CourseService CourseService, InstructorService InstructorService, ExamQuestionService ExamQuestionService,QuestionService QuestionService,
            ExamStudentService examStudentService)
        {
            _ExamRepo = ExamRepo;
            _CourseService = CourseService;
            _InstructorService = InstructorService;
            _ExamQuestionService = ExamQuestionService;
            _QuestionService = QuestionService;
            _ExamStudentService = examStudentService;
        }

        //------------------------------------------------------

        private decimal EvaluateStudentAnswers(SubmitExamDTO StudentAnswers , ICollection<GetQuestionWithCorrectAnswerDTO> CorrectAnswers)
        {

            //(key/value) ====> (QuestionId/(QuestionId,ChoiceId,Grade)), the value is the entire object because we need Grade and Correct Choice id At the same Time
            var _CorrectAnsersDictionary = CorrectAnswers.ToDictionary(x => x.QuestionId, x => x);

            decimal TotalGrade = 0;

            foreach (var item in StudentAnswers.Answers)
            {
                if (_CorrectAnsersDictionary.TryGetValue(item.QuestionId, out var ValueOfTheKey) && ValueOfTheKey.ChoiceId == item.ChoiceId)
                {
                    TotalGrade += ValueOfTheKey.Grade;
                }
            }

            return TotalGrade;
        }

        //------------------------------------------------------

        public async Task<ResponseViewModel<bool>> AddAsync(CreateExamDTO model)
        {
            if (model is null || model.CourseId <= 0 || model.InstructorId <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid exam input");

            var isCourseExist = await _CourseService.IsExist(model.CourseId);
            if (!isCourseExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.CourseNotFound, "Course not found");

            var isInstructorExist = await _InstructorService.IsExist(model.InstructorId);
            if (!isInstructorExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.InstructorNotFound, "Instructor not found");

            var newExamModel = model.Map<Exam>();
            var result = await _ExamRepo.AddAsync(newExamModel);

            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamAddFail, "Failed to add exam");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Exam added successfully");
        }

        public async Task<bool> IsExist(int id)
        {
            return await _ExamRepo.AnyAsync(ex => ex.ID == id && ex.Deleted == false);
        }

        public async Task<ResponseViewModel<bool>> DeleteAsync(int id)
        {
            if (id <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid exam id");

            var isExist = await IsExist(id);
            if (!isExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var result = await _ExamRepo.DeleteAsync(id);
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamDeleteFail, "Failed to delete exam");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Exam deleted successfully");
        }

        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateExamDTO model)
        {
            if (model is null || model.ID <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid exam input");

            var isExist = await IsExist(model.ID);
            if (!isExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var updateModel = model.Map<Exam>();

            var result = await _ExamRepo.UpdateInclude(
                updateModel,
                nameof(Exam.Name),
                nameof(Exam.Type),
                nameof(Exam.Date),
                nameof(Exam.DurationInMinutes));

            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamUpdateFail, "Failed to update exam");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Exam updated successfully");
        }

        public async Task<ResponseViewModel<bool>> AssignQuestionToExam(AssignQuestionToExamDTO model)
        {
            if (model is null || model.ExamId <= 0 || model.QuestionId <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid input");

            var isExamExist = await IsExist(model.ExamId);
            if (!isExamExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var isQuestionExist = await _QuestionService.IsExistAsync(model.QuestionId);
            if (!isQuestionExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.QustionNotFound, "Question not found");

            var isQuestionDuplicated = await _ExamQuestionService.IsQuestionExistOnExam(model.ExamId, model.QuestionId);
            if (isQuestionDuplicated)
                return ResponseViewModel<bool>.Failure(ErrorCode.QuestionAlreadyAssignedToExam, "Question already assigned to this exam");

            var result = await _ExamQuestionService.AddAsync(model);
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.AssignQuestionToExamFail, "Failed to assign question to exam");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Question assigned to exam successfully");
        }

        public async Task<ResponseViewModel<bool>> UpdateQuestionOnExam(UpdateExamQuestionDTO model)
        {
            if (model is null || model.ID <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid input");

            var result = await _ExamQuestionService.UpdateQuestionOnExam(model);
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.AssignQuestionToExamFail, "Failed to update question on exam");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Question updated on exam successfully");
        }

        public async Task<ResponseViewModel<bool>> DeleteQuestoinFromExam(int examQuestionRecordId)
        {
            if (examQuestionRecordId <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid exam-question record id");

            var isExist = await _ExamQuestionService.IsExist(examQuestionRecordId);
            if (!isExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamQuestionRecordNotFound, "Exam-question record not found");

            var result = await _ExamQuestionService.DeleteQuestionFromExam(examQuestionRecordId);
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.DeleteQuestionFromExamFail, "Failed to delete question from exam");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Question removed from exam successfully");
        }

        public async Task<ResponseViewModel<bool>> AssignStudentToExam(CreateExamStudentDTO model)
        {
            if (model is null || model.ExamId <= 0 || model.StudentId <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid input");

            var isExamExist = await IsExist(model.ExamId);
            if (!isExamExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var isAssignedBefore = await _ExamStudentService.IsStudentAssignedToExamAsync(model.ExamId, model.StudentId);
            if (isAssignedBefore)
                return ResponseViewModel<bool>.Failure(ErrorCode.StudentAlreadyAssignedToExam, "Student already assigned to this exam");

            var result = await _ExamStudentService.AddAsync(model);
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.AssignStudentToExamFail, "Failed to assign student to exam");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Student assigned to exam successfully");
        }

        public async Task<ResponseViewModel<ExamViewDTO>> ViewExam(int examId)
        {
            if (examId <= 0)
                return ResponseViewModel<ExamViewDTO>.Failure(ErrorCode.InvalidExamInput, "Invalid exam id");

            var examDetails = await _ExamRepo.Get(x => x.ID == examId && x.Deleted == false)
                .Select(ex => new ExamViewDTO
                {
                    Name = ex.Name,
                    Type = ex.Type,
                    DurationInMinutes = ex.DurationInMinutes,
                    Date = ex.Date,
                    AllQuestion = new List<GetQuestionDTO>()
                })
                .FirstOrDefaultAsync();

            if (examDetails is null)
                return ResponseViewModel<ExamViewDTO>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            examDetails.AllQuestion = await _ExamQuestionService.GetExamQuestionsByExamId(examId);

            return ResponseViewModel<ExamViewDTO>.Success(examDetails, ErrorCode.None, "Exam retrieved successfully");
        }

        public async Task<ResponseViewModel<decimal>> SubmitExam(SubmitExamDTO StudentAnswers)
        {
            if (StudentAnswers is null ||
                StudentAnswers.ExamId <= 0 ||
                StudentAnswers.StudentId <= 0 ||
                StudentAnswers.Answers is null ||
                StudentAnswers.Answers.Count == 0)
            {
                return ResponseViewModel<decimal>.Failure(ErrorCode.InvalidExamInput, "Invalid submit exam input");
            }

            var isExamExist = await IsExist(StudentAnswers.ExamId);
            if (!isExamExist)
                return ResponseViewModel<decimal>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var isStudentAssigned = await _ExamStudentService.IsStudentAssignedToExamAsync(StudentAnswers.ExamId, StudentAnswers.StudentId);
            if (!isStudentAssigned)
                return ResponseViewModel<decimal>.Failure(ErrorCode.StudentNotAssignedToExam, "Student is not assigned to this exam");

            // Get All Questions and Correct choices of this exam (we will use it to evaluate Student Answers)

            var QuestionsWithCorrectChoices = await _ExamQuestionService.GetExamQuestionsWithCorrectAnswersByExamId(StudentAnswers.ExamId);
            if (QuestionsWithCorrectChoices.Count == 0)
                return ResponseViewModel<decimal>.Failure(ErrorCode.NoQuestionsAssignedToExam, "No questions found for this exam");

            // Evaluate Student Question Answers

            var FinalGrade = this.EvaluateStudentAnswers(StudentAnswers, QuestionsWithCorrectChoices);

            //Save student Grade for the exam on (StudentExam)
            var saved = await _ExamStudentService.SaveFinalGrade(StudentAnswers.ExamId, StudentAnswers.StudentId, FinalGrade);

            if (!saved)
                return ResponseViewModel<decimal>.Failure(ErrorCode.SubmitExamFail, "Failed to submit exam");

            // return Grade
            return ResponseViewModel<decimal>.Success(FinalGrade, ErrorCode.None, "Exam submitted successfully");
        }

        public async Task<ResponseViewModel<bool>> RandomExam(CreateRandomExamDTO model)
        {
            // 1. Validation
            if (model is null || model.ExamId <= 0 || model.CourseId <= 0 || model.QuestionsConfig is null || !model.QuestionsConfig.Any())
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid input configuration");

            if (model.QuestionsConfig.Any(c => c.Count <= 0 || c.GradePerQuestion <= 0))
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid question config values");

            var isExamExist = await IsExist(model.ExamId);
            if (!isExamExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var isCourseExist = await _CourseService.IsExist(model.CourseId);
            if (!isCourseExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.CourseNotFound, "Course not found");

            // 2. Random selection from DB per level
            var selectedQuestionIds = new HashSet<int>();
            var questionsToAssign = new List<AssignQuestionToExamDTO>();

            foreach (var config in model.QuestionsConfig)
            {
                var randomQuestionIds = await _QuestionService.GetRandomQuestionIdsByCourseAndLevelAsync(
                    model.CourseId,
                    config.Level,
                    config.Count,
                    selectedQuestionIds.ToList());

                if (randomQuestionIds.Count < config.Count)
                    return ResponseViewModel<bool>.Failure(
                        ErrorCode.InvalidExamInput,
                        $"Not enough questions available for level {config.Level}. Requested: {config.Count}, Available: {randomQuestionIds.Count}");

                foreach (var questionId in randomQuestionIds)
                {
                    selectedQuestionIds.Add(questionId);

                    questionsToAssign.Add(new AssignQuestionToExamDTO
                    {
                        ExamId = model.ExamId,
                        QuestionId = questionId,
                        Grade = config.GradePerQuestion
                    });
                }
            }

            // 3. assign Questions to exam
            try
            {
                var assignResult = await _ExamQuestionService.AddRangeAsync(questionsToAssign);
                if (!assignResult)
                    return ResponseViewModel<bool>.Failure(ErrorCode.AssignQuestionToExamFail, "Failed to assign questions to the exam");

                return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Random exam generated and questions assigned successfully");
            }
            catch (Exception)
            {
                return ResponseViewModel<bool>.Failure(ErrorCode.AssignQuestionToExamFail, "Unexpected error occurred while assigning questions");
            }
        }
    }
}
