using ExaminationSystem.BLL.DTOs.Exam;
using ExaminationSystem.BLL.DTOs.ExamQuestion;
using ExaminationSystem.BLL.DTOs.ExamStudent;
using ExaminationSystem.BLL.DTOs.Question;
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
    public class ExamService : IExamService
    {
        private readonly IRepository<Exam> _ExamRepo;
        private readonly ICourseService _CourseService;
        private readonly IInstructorService _InstructorService;
        private readonly IExamQuestionService _ExamQuestionService;
        private readonly IQuestionService _QuestionService;
        private readonly IExamStudentService _ExamStudentService;

        public ExamService(IRepository<Exam> ExamRepo, ICourseService CourseService, IInstructorService InstructorService, IExamQuestionService ExamQuestionService, IQuestionService QuestionService, IExamStudentService examStudentService)
        {
            _ExamRepo = ExamRepo;
            _CourseService = CourseService;
            _InstructorService = InstructorService;
            _ExamQuestionService = ExamQuestionService;
            _QuestionService = QuestionService;
            _ExamStudentService = examStudentService;
        }

        private decimal EvaluateStudentAnswers(SubmitExamDTO StudentAnswers, ICollection<GetQuestionWithCorrectAnswerDTO> CorrectAnswers)
        {
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

        public async Task<ResponseViewModel<ExamViewDTO>> AddAsync(CreateExamDTO model, CancellationToken cancellationToken = default)
        {
            if (model is null || model.CourseId <= 0 || model.InstructorId <= 0)
                return ResponseViewModel<ExamViewDTO>.Failure(ErrorCode.InvalidExamInput, "Invalid exam input");

            var isCourseExist = await _CourseService.IsExist(model.CourseId, cancellationToken);
            if (!isCourseExist)
                return ResponseViewModel<ExamViewDTO>.Failure(ErrorCode.CourseNotFound, "Course not found");

            var isInstructorExist = await _InstructorService.IsExist(model.InstructorId, cancellationToken);
            if (!isInstructorExist)
                return ResponseViewModel<ExamViewDTO>.Failure(ErrorCode.InstructorNotFound, "Instructor not found");

            var newExamModel = model.Map<Exam>();
            await _ExamRepo.AddAsync(newExamModel, cancellationToken);
            var result = await _ExamRepo.SaveChangesAsync(cancellationToken);

            if (!result)
                return ResponseViewModel<ExamViewDTO>.Failure(ErrorCode.SaveExamFail, "Failed to save exam");

            return ResponseViewModel<ExamViewDTO>.Success(newExamModel.Map<ExamViewDTO>(), ErrorCode.None, "Exam added successfully");
        }

        public async Task<bool> IsExist(int id, CancellationToken cancellationToken = default)
        {
            return await _ExamRepo.CheckExistsByConditionAsync(ex => ex.ID == id && ex.Deleted == false, cancellationToken);
        }

        public async Task<ResponseViewModel<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid exam id");

            var isExist = await IsExist(id, cancellationToken);
            if (!isExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var exam = await _ExamRepo.GetByIDAsync(id, cancellationToken);
            if (exam == null) return ResponseViewModel<bool>.Failure(ErrorCode.ExamDeleteFail, "Failed to delete exam");

            _ExamRepo.SoftDelete(exam);
            var result = await _ExamRepo.SaveChangesAsync(cancellationToken);
            
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.SaveExamFail, "Failed to save exam deletion");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Exam deleted successfully");
        }

        public async Task<ResponseViewModel<ExamViewDTO>> UpdateAsync(UpdateExamDTO model, CancellationToken cancellationToken = default)
        {
            if (model is null || model.ID <= 0)
                return ResponseViewModel<ExamViewDTO>.Failure(ErrorCode.InvalidExamInput, "Invalid exam input");

            var isExist = await IsExist(model.ID, cancellationToken);
            if (!isExist)
                return ResponseViewModel<ExamViewDTO>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var updateModel = model.Map<Exam>();

            _ExamRepo.UpdateInclude(
                updateModel,
                nameof(Exam.Name),
                nameof(Exam.Type),
                nameof(Exam.Date),
                nameof(Exam.DurationInMinutes));
                
            var result = await _ExamRepo.SaveChangesAsync(cancellationToken);

            if (!result)
                return ResponseViewModel<ExamViewDTO>.Failure(ErrorCode.SaveExamFail, "Failed to save exam updates");

            return ResponseViewModel<ExamViewDTO>.Success(updateModel.Map<ExamViewDTO>(), ErrorCode.None, "Exam updated successfully");
        }

        public async Task<ResponseViewModel<bool>> AssignQuestionToExam(AssignQuestionToExamDTO model, CancellationToken cancellationToken = default)
        {
            if (model is null || model.ExamId <= 0 || model.QuestionId <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid input");

            var isExamExist = await IsExist(model.ExamId, cancellationToken);
            if (!isExamExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var isQuestionExist = await _QuestionService.IsExistAsync(model.QuestionId, cancellationToken);
            if (!isQuestionExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.QustionNotFound, "Question not found");

            var isQuestionDuplicated = await _ExamQuestionService.IsQuestionExistOnExam(model.ExamId, model.QuestionId, cancellationToken);
            if (isQuestionDuplicated)
                return ResponseViewModel<bool>.Failure(ErrorCode.QuestionAlreadyAssignedToExam, "Question already assigned to this exam");

            var result = await _ExamQuestionService.AddAsync(model, cancellationToken);
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.AssignQuestionToExamFail, "Failed to assign question to exam");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Question assigned to exam successfully");
        }

        public async Task<ResponseViewModel<bool>> UpdateQuestionOnExam(UpdateExamQuestionDTO model, CancellationToken cancellationToken = default)
        {
            if (model is null || model.ID <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid input");

            var result = await _ExamQuestionService.UpdateQuestionOnExam(model, cancellationToken);
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.AssignQuestionToExamFail, "Failed to update question on exam");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Question updated on exam successfully");
        }

        public async Task<ResponseViewModel<bool>> DeleteQuestoinFromExam(int examQuestionRecordId, CancellationToken cancellationToken = default)
        {
            if (examQuestionRecordId <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid exam-question record id");

            var isExist = await _ExamQuestionService.IsExist(examQuestionRecordId, cancellationToken);
            if (!isExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamQuestionRecordNotFound, "Exam-question record not found");

            var result = await _ExamQuestionService.DeleteQuestionFromExam(examQuestionRecordId, cancellationToken);
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.DeleteQuestionFromExamFail, "Failed to delete question from exam");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Question removed from exam successfully");
        }

        public async Task<ResponseViewModel<bool>> AssignStudentToExam(CreateExamStudentDTO model, CancellationToken cancellationToken = default)
        {
            if (model is null || model.ExamId <= 0 || model.StudentId <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid input");

            var isExamExist = await IsExist(model.ExamId, cancellationToken);
            if (!isExamExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var isAssignedBefore = await _ExamStudentService.IsStudentAssignedToExamAsync(model.ExamId, model.StudentId, cancellationToken);
            if (isAssignedBefore)
                return ResponseViewModel<bool>.Failure(ErrorCode.StudentAlreadyAssignedToExam, "Student already assigned to this exam");

            var result = await _ExamStudentService.AddAsync(model, cancellationToken);
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.AssignStudentToExamFail, "Failed to assign student to exam");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Student assigned to exam successfully");
        }

        public async Task<ResponseViewModel<ExamViewDTO>> ViewExam(int examId, CancellationToken cancellationToken = default)
        {
            if (examId <= 0)
                return ResponseViewModel<ExamViewDTO>.Failure(ErrorCode.InvalidExamInput, "Invalid exam id");

            var examDetails = await _ExamRepo.GetByCondition(x => x.ID == examId && x.Deleted == false)
                .Select(ex => new ExamViewDTO
                {
                    Name = ex.Name,
                    Type = ex.Type,
                    DurationInMinutes = ex.DurationInMinutes,
                    Date = ex.Date,
                    AllQuestion = new List<GetQuestionDTO>()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (examDetails is null)
                return ResponseViewModel<ExamViewDTO>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            examDetails.AllQuestion = await _ExamQuestionService.GetExamQuestionsByExamId(examId, cancellationToken);

            return ResponseViewModel<ExamViewDTO>.Success(examDetails, ErrorCode.None, "Exam retrieved successfully");
        }

        public async Task<ResponseViewModel<decimal>> SubmitExam(SubmitExamDTO StudentAnswers, CancellationToken cancellationToken = default)
        {
            if (StudentAnswers is null ||
                StudentAnswers.ExamId <= 0 ||
                StudentAnswers.StudentId <= 0 ||
                StudentAnswers.Answers is null ||
                StudentAnswers.Answers.Count == 0)
            {
                return ResponseViewModel<decimal>.Failure(ErrorCode.InvalidExamInput, "Invalid submit exam input");
            }

            var isExamExist = await IsExist(StudentAnswers.ExamId, cancellationToken);
            if (!isExamExist)
                return ResponseViewModel<decimal>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var isStudentAssigned = await _ExamStudentService.IsStudentAssignedToExamAsync(StudentAnswers.ExamId, StudentAnswers.StudentId, cancellationToken);
            if (!isStudentAssigned)
                return ResponseViewModel<decimal>.Failure(ErrorCode.StudentNotAssignedToExam, "Student is not assigned to this exam");

            var QuestionsWithCorrectChoices = await _ExamQuestionService.GetExamQuestionsWithCorrectAnswersByExamId(StudentAnswers.ExamId, cancellationToken);
            if (QuestionsWithCorrectChoices.Count == 0)
                return ResponseViewModel<decimal>.Failure(ErrorCode.NoQuestionsAssignedToExam, "No questions found for this exam");

            var FinalGrade = this.EvaluateStudentAnswers(StudentAnswers, QuestionsWithCorrectChoices);

            var saved = await _ExamStudentService.SaveFinalGrade(StudentAnswers.ExamId, StudentAnswers.StudentId, FinalGrade, cancellationToken);

            if (!saved)
                return ResponseViewModel<decimal>.Failure(ErrorCode.SubmitExamFail, "Failed to submit exam");

            return ResponseViewModel<decimal>.Success(FinalGrade, ErrorCode.None, "Exam submitted successfully");
        }

        public async Task<ResponseViewModel<bool>> RandomExam(CreateRandomExamDTO model, CancellationToken cancellationToken = default)
        {
            if (model is null || model.ExamId <= 0 || model.CourseId <= 0 || model.QuestionsConfig is null || !model.QuestionsConfig.Any())
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid input configuration");

            if (model.QuestionsConfig.Any(c => c.Count <= 0 || c.GradePerQuestion <= 0))
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidExamInput, "Invalid question config values");

            var isExamExist = await IsExist(model.ExamId, cancellationToken);
            if (!isExamExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.ExamNotFound, "Exam not found");

            var isCourseExist = await _CourseService.IsExist(model.CourseId, cancellationToken);
            if (!isCourseExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.CourseNotFound, "Course not found");

            var selectedQuestionIds = new HashSet<int>();
            var questionsToAssign = new List<AssignQuestionToExamDTO>();

            foreach (var config in model.QuestionsConfig)
            {
                var randomQuestionIds = await _QuestionService.GetRandomQuestionIdsByCourseAndLevelAsync(
                    model.CourseId,
                    config.Level,
                    config.Count,
                    selectedQuestionIds.ToList(),
                    cancellationToken);

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

            try
            {
                var assignResult = await _ExamQuestionService.AddRangeAsync(questionsToAssign, cancellationToken);
                if (!assignResult)
                    return ResponseViewModel<bool>.Failure(ErrorCode.AssignQuestionToExamFail, "Failed to assign questions to the exam");

                return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Random exam generated and questions assigned successfully");
            }
            catch (Exception)
            {
                return ResponseViewModel<bool>.Failure(ErrorCode.AssignQuestionToExamFail, "Unexpected error occurred while assigning questions");
            }
        }

        public async Task<ResponseViewModel<IEnumerable<ViewStudentsGradesDTO>>> ViewStudentsGrades(int ExamId, CancellationToken cancellationToken = default)
        {
            var IsExamExist = await this.IsExist(ExamId, cancellationToken);

            if (IsExamExist)
                return await _ExamStudentService.ViewStudentsGrades(ExamId, cancellationToken);
            else
                return ResponseViewModel<IEnumerable<ViewStudentsGradesDTO>>.Failure(ErrorCode.ExamNotFound, "Exam Not Found");
        }

        public async Task<ResponseViewModel<decimal?>> TopGrade(int ExamId, CancellationToken cancellationToken = default)
        {
            var IsExamExist = await this.IsExist(ExamId, cancellationToken);

            if (IsExamExist)
                return await _ExamStudentService.TopGrade(ExamId, cancellationToken);
            else
                return ResponseViewModel<decimal?>.Failure(ErrorCode.ExamNotFound, "Exam Not Found");
        }

        public async Task<ResponseViewModel<decimal?>> AverageGrade(int ExamId, CancellationToken cancellationToken = default)
        {
            var IsExamExist = await this.IsExist(ExamId, cancellationToken);

            if (IsExamExist)
                return await _ExamStudentService.AverageGrade(ExamId, cancellationToken);
            else
                return ResponseViewModel<decimal?>.Failure(ErrorCode.ExamNotFound, "Exam Not Found");
        }
    }
}
