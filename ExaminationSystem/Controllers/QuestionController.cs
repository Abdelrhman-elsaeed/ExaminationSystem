using ExaminationSystem.Enums;
using ExaminationSystem.Helper;
using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.ModelDTO.Question;
using ExaminationSystem.Models;
using ExaminationSystem.ModelVm.Question;
using ExaminationSystem.Repo;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels;
using ExaminationSystem.ViewModels.Question;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ExaminationSystem.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _QuestionService;
        public QuestionController(QuestionService QuestionService)
        {
            _QuestionService = QuestionService;
        }

        [HttpPut]
        public async Task<ResponseViewModel<CreateQuestionVM>> Add(CreateQuestionVM model)
        {
            // map VM to DTO
            var newQuestionDto = model.Map<CreateQuestionDTO>();
            var result = await _QuestionService.AddAsync(newQuestionDto);

            if (result)
                return ResponseViewModel<CreateQuestionVM>.Success(model,message: "Question added successfully.");
            else
                return ResponseViewModel<CreateQuestionVM>.Failure(ErrorCode.None,"Question was not added successfully.");
        }

        [HttpDelete]
        public async Task<ResponseViewModel<bool>> Delete(int id)
        {
            var result = await _QuestionService.DeleteQuestionAndChoicesAsync(id);

            if (result)
                return ResponseViewModel<bool>.Success(result, message: "Question and Choices Deleted Successfully");
            else
                return ResponseViewModel<bool>.Failure(ErrorCode.QustionNotFound,"Unable to delete Question and Choices");

        }

        [HttpGet]
        public async Task<ResponseViewModel<List<GetAllQuestionVM>>> GetAll()
        {

            // here T = List<GetAllQuestionVM>

            var allQuestionDTOs = await _QuestionService.GetAllAsync();

            if (allQuestionDTOs is null || !allQuestionDTOs.Any())
            {
                return ResponseViewModel<List<GetAllQuestionVM>>.Failure(ErrorCode.QustionNotFound, message: "Questions Not Found");
            }

            var allQuestionVM = allQuestionDTOs.Select(q => q.Map<GetAllQuestionVM>()).ToList();

            return ResponseViewModel<List<GetAllQuestionVM>>.Success(allQuestionVM, message: "All Questions Returived Successfully");


        }

        [HttpPatch]
        public async Task<ResponseViewModel<bool>> UpdateQuestion(UpdateQuestionVM model)
        {

            var UpdatesDTO = model.Map<UpdateQuestionDTO>();

            var result = await _QuestionService.UpdateQuestionAsync(UpdatesDTO);

            if (result)
                return ResponseViewModel<bool>.Success(result, message: "Questoin Updated Successfully");
            else
                return ResponseViewModel<bool>.Failure(ErrorCode.QuestionUpdateFail, message: "Questoin Fail to Update Successfully");
        }

    }
}
