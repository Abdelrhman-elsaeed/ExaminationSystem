using ExaminationSystem.DTOs.ExamQuestion;
using ExaminationSystem.Helper;
using ExaminationSystem.ModelDTO.Exam;
using ExaminationSystem.ModelDTO.ExamQuestion;
using ExaminationSystem.Models;
using ExaminationSystem.Repo;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.Exam;
using ExaminationSystem.ViewModels.ExamQuestion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Reflection.Metadata;

namespace ExaminationSystem.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class ExamController : ControllerBase
    {
        private readonly ExamService _ExamService;
        public ExamController(ExamService ExamService)
        {
            _ExamService = ExamService;
        }


        [HttpPut]
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
        public async Task<IActionResult> DeleteExam(int id)
        {
            var resutl = await _ExamService.DeleteAsync(id);

            if (resutl.IsSuccess)
                return Ok(resutl);
            else
                return NotFound(resutl);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateQuestionOnExam(UpdateExamQuestionVM model)
        {
            var UpdateDTO = model.Map<UpdateExamQuestionDTO>();

            var resutl = await _ExamService.UpdateQuestionOnExam(UpdateDTO);

            if (resutl.IsSuccess)
                return Ok(resutl);
            else
                return NotFound(resutl);
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteQuestionFromExam(int id)
        //{

        //}

        // exam view , random , report results , submit

    }
}
