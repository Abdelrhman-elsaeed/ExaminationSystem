namespace ExaminationSystem.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class ExamController : ControllerBase
    {
        private readonly ExamService _ExamService;
        public ExamController(ExamService ExamService)
        {
            _ExamService = ExamService;
        }


        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.AddExam })]
        public async Task<IActionResult> Add(CreateExamVM model)
        {
            var CreateExamDTO = model.Map<CreateExamDTO>();

            var resutl = await _ExamService.AddAsync(CreateExamDTO);

            if (resutl.IsSuccess)
                return Ok(resutl);
            else
                return NotFound(resutl);
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.AssignStudentToExam })]
        public async Task<IActionResult> AssignStudentToExam(CreateExamStudentVM model)
        {
            var ExamStudentDTO = model.Map<CreateExamStudentDTO>();
            var result = await _ExamService.AssignStudentToExam(ExamStudentDTO);

            if (result.IsSuccess)
                return Ok(result);
            else
                return NotFound(result);
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.AssignQuestionToExam })]
        public async Task<IActionResult> AssignQuestionToExam(AssignQuestionToExamVM model)
        {
            var AssignQuesionDTO = model.Map<AssignQuestionToExamDTO>();

            var result = await _ExamService.AssignQuestionToExam(AssignQuesionDTO);

            if (result.IsSuccess)
                return Ok(result);
            else
                return NotFound(result);

        }

        [HttpPatch]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.UpdateExam })]
        public async Task<IActionResult> Update(UpdateExamVM model)
        {
            var UpdateDTO = model.Map<UpdateExamDTO>();

            var result = await _ExamService.UpdateAsync(UpdateDTO);

            if (result.IsSuccess)
                return Ok(result);
            else
                return NotFound(result);
        }

        [HttpDelete]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.DeleteExam })]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var resutl = await _ExamService.DeleteAsync(id);

            if (resutl.IsSuccess)
                return Ok(resutl);
            else
                return NotFound(resutl);
        }

        [HttpPatch]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.UpdateQuestionOnExam })]
        public async Task<IActionResult> UpdateQuestionOnExam(UpdateExamQuestionVM model)
        {
            var UpdateDTO = model.Map<UpdateExamQuestionDTO>();

            var resutl = await _ExamService.UpdateQuestionOnExam(UpdateDTO);

            if (resutl.IsSuccess)
                return Ok(resutl);
            else
                return NotFound(resutl);
        }

        [HttpDelete]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.DeleteQuestionFromExam })]
        public async Task<IActionResult> DeleteQuestionFromExam(int id)
        {
            var result = await _ExamService.DeleteQuestoinFromExam(id);

            if (result.IsSuccess)
                return Ok(result);
            else
                return NotFound(result);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.ViewExam })]
        public async Task<IActionResult> ViewExam(int ExamId)
        {
            var ViewExamDTO = await _ExamService.ViewExam(ExamId);


            if (ViewExamDTO.IsSuccess)
            {
                var ViewExamVM = ViewExamDTO.Data.Map<ExamViewVM>();
                return Ok(ResponseViewModel<ExamViewVM>.Success(ViewExamVM, ViewExamDTO.ErrorCode, ViewExamDTO.Message));
            }

            else
            {
                return NotFound(ResponseViewModel<ExamViewVM>.Failure(ViewExamDTO.ErrorCode, ViewExamDTO.Message));
            }


        }

        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.SubmitExam })]
        public async Task<IActionResult> SubmitExam(SubmitExamVM model)
        {
            var SubmitExamDTO = model.Map<SubmitExamDTO>();
            var result = await _ExamService.SubmitExam(SubmitExamDTO);

            if (result.IsSuccess)
                return Ok(result);
            else
                return NotFound(result);
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.CreateRandomExam })]
        public async Task<IActionResult> CreateRandomExam(CreateRandomExamVM model)
        {
            var RandomExamDTO = model.Map<CreateRandomExamDTO>();

            var result = await _ExamService.RandomExam(RandomExamDTO);

            if (result.IsSuccess)
                return Ok(result);
            else
                return NotFound(result);

        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.ViewStudentsGrades })]
        public async Task<IActionResult> ViewStudentsGrades(int ExamId)
        {
            var StudentsGradesDTO = await _ExamService.ViewStudentsGrades(ExamId);

            if(StudentsGradesDTO.IsSuccess)
            {
                var StudentsGradesDataVM = StudentsGradesDTO.Data.Map<IEnumerable<ViewStudentsGradesVM>>();
                return Ok(ResponseViewModel<IEnumerable<ViewStudentsGradesVM>>.Success(StudentsGradesDataVM, StudentsGradesDTO.ErrorCode, StudentsGradesDTO.Message));
            }
            else
            {
                return NotFound(ResponseViewModel<IEnumerable<ViewStudentsGradesVM>>.Failure(StudentsGradesDTO.ErrorCode, StudentsGradesDTO.Message));
            }
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.TopGrade })]
        public async Task<IActionResult> TopGrade(int ExamId)
        {
            var StudentsTopGrades = await _ExamService.TopGrade(ExamId);

            if (StudentsTopGrades.IsSuccess)
                return Ok(StudentsTopGrades);
            else
                return
                    NotFound(StudentsTopGrades);

        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeFilter), Arguments = new object[] { Feature.AverageGrade })]
        public async Task<IActionResult> AverageGrade(int ExamId)
        {
            var StudentAverageGrades = await _ExamService.AverageGrade(ExamId);

            if (StudentAverageGrades.IsSuccess)
                return Ok(StudentAverageGrades);
            else
                return
                    NotFound(StudentAverageGrades);

        }

    }
}
