using ExaminationSystem.BLL.DTOs.Exam;
using ExaminationSystem.BLL.DTOs.ExamQuestion;
using ExaminationSystem.BLL.DTOs.ExamStudent;
using ExaminationSystem.BLL.ViewModels;
using ExaminationSystem.BLL.ViewModels.Exam;
using ExaminationSystem.BLL.ViewModels.ExamQuestion;
using ExaminationSystem.BLL.ViewModels.ExamStudent;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _ExamService;
        private readonly IExamStudentService _examStudentService;

        public ExamController(IExamService ExamService, IExamStudentService examStudentService)
        {
            _ExamService = ExamService;
            _examStudentService = examStudentService;
        }

        [HttpPut]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Add(CreateExamVM model, CancellationToken cancellationToken)
        {
            var CreateExamDTO = model.Map<CreateExamDTO>();

            var resutl = await _ExamService.AddAsync(CreateExamDTO, cancellationToken);

            if (resutl.IsSuccess)
                return Ok(ResponseViewModel<ExamViewVM>.Success(resutl.Data.Map<ExamViewVM>(), message: resutl.Message));

            return NotFound(ResponseViewModel<ExamViewVM>.Failure(resutl.ErrorCode, message: resutl.Message));
        }

        [HttpPut]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AssignStudentToExam(CreateExamStudentVM model, CancellationToken cancellationToken)
        {
            var ExamStudentDTO = model.Map<CreateExamStudentDTO>();
            var result = await _ExamService.AssignStudentToExam(ExamStudentDTO, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);

            return NotFound(result);
        }

        [HttpPut]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AssignQuestionToExam(AssignQuestionToExamVM model, CancellationToken cancellationToken)
        {
            var AssignQuesionDTO = model.Map<AssignQuestionToExamDTO>();

            var result = await _ExamService.AssignQuestionToExam(AssignQuesionDTO, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);

            return NotFound(result);
        }

        [HttpPatch]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Update(UpdateExamVM model, CancellationToken cancellationToken)
        {
            var UpdateDTO = model.Map<UpdateExamDTO>();

            var result = await _ExamService.UpdateAsync(UpdateDTO, cancellationToken);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<ExamViewVM>.Success(result.Data.Map<ExamViewVM>(), message: result.Message));

            return NotFound(ResponseViewModel<ExamViewVM>.Failure(result.ErrorCode, message: result.Message));
        }

        [HttpDelete]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteExam(int id, CancellationToken cancellationToken)
        {
            var resutl = await _ExamService.DeleteAsync(id, cancellationToken);

            if (resutl.IsSuccess)
                return Ok(resutl);

            return NotFound(resutl);
        }

        [HttpPatch]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateQuestionOnExam(UpdateExamQuestionVM model, CancellationToken cancellationToken)
        {
            var UpdateDTO = model.Map<UpdateExamQuestionDTO>();

            var resutl = await _ExamService.UpdateQuestionOnExam(UpdateDTO, cancellationToken);

            if (resutl.IsSuccess)
                return Ok(resutl);

            return NotFound(resutl);
        }

        [HttpDelete]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteQuestionFromExam(int id, CancellationToken cancellationToken)
        {
            var result = await _ExamService.DeleteQuestoinFromExam(id, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ViewExam(int ExamId, CancellationToken cancellationToken)
        {
            var ViewExamDTO = await _ExamService.ViewExam(ExamId, cancellationToken);

            if (ViewExamDTO.IsSuccess)
                return Ok(ResponseViewModel<ExamViewVM>.Success(ViewExamDTO.Data.Map<ExamViewVM>(), ViewExamDTO.ErrorCode, ViewExamDTO.Message));

            return NotFound(ResponseViewModel<ExamViewVM>.Failure(ViewExamDTO.ErrorCode, ViewExamDTO.Message));
        }

        [HttpPut]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitExam(SubmitExamVM model, CancellationToken cancellationToken)
        {
            var SubmitExamDTO = model.Map<SubmitExamDTO>();
            var result = await _ExamService.SubmitExam(SubmitExamDTO, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);

            return NotFound(result);
        }

        [HttpPut]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> CreateRandomExam(CreateRandomExamVM model, CancellationToken cancellationToken)
        {
            var RandomExamDTO = model.Map<CreateRandomExamDTO>();

            var result = await _ExamService.RandomExam(RandomExamDTO, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> ViewStudentsGrades(int ExamId, CancellationToken cancellationToken)
        {
            var StudentsGradesDTO = await _ExamService.ViewStudentsGrades(ExamId, cancellationToken);

            if(StudentsGradesDTO.IsSuccess)
                return Ok(ResponseViewModel<IEnumerable<ViewStudentsGradesVM>>.Success(StudentsGradesDTO.Data.Map<IEnumerable<ViewStudentsGradesVM>>(), StudentsGradesDTO.ErrorCode, StudentsGradesDTO.Message));

            return NotFound(ResponseViewModel<IEnumerable<ViewStudentsGradesVM>>.Failure(StudentsGradesDTO.ErrorCode, StudentsGradesDTO.Message));
        }

        [HttpGet]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> TopGrade(int ExamId, CancellationToken cancellationToken)
        {
            var StudentsTopGrades = await _ExamService.TopGrade(ExamId, cancellationToken);

            if (StudentsTopGrades.IsSuccess)
                return Ok(StudentsTopGrades);

            return NotFound(StudentsTopGrades);
        }

        [HttpGet]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AverageGrade(int ExamId, CancellationToken cancellationToken)
        {
            var StudentAverageGrades = await _ExamService.AverageGrade(ExamId, cancellationToken);

            if (StudentAverageGrades.IsSuccess)
                return Ok(StudentAverageGrades);

            return NotFound(StudentAverageGrades);
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ViewMyGrade(int studentId, int examId, CancellationToken cancellationToken)
        {
            var result = await _examStudentService.StudentFinalGrade(studentId, examId, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);

            return NotFound(result);
        }
    }
}
