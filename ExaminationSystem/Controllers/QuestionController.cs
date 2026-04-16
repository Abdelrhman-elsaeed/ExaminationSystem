using ExaminationSystem.ModelDTO.Choice;
using ExaminationSystem.ModelDTO.Question;
using ExaminationSystem.Models;
using ExaminationSystem.ModelVm.Question;
using ExaminationSystem.Repo;
using ExaminationSystem.Repo.RepositoryExtension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ExaminationSystem.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class QuestionController : ControllerBase
    {

        private readonly QuestionRepo _QuestionRepo;
        private readonly GenericRepository<Choice> _ChoiceRepo;
        public QuestionController(QuestionRepo QuestionRepo, GenericRepository<Choice> ChoiceRepo)
        {
            _QuestionRepo = QuestionRepo;
            _ChoiceRepo = ChoiceRepo;
        }

        [HttpPut]
        public async Task<IActionResult> Add(CreateQuestionDTO model)
        {

            //mapping 

            var NewQuestion = new Question()
            {
                Title = model.Title,
                CourseId = model.CourseId,
                InstructorId = model.InstructorId,
                Level = model.Level,
                Choices = model.Choices.Select(item => new Choice
                {
                    Text = item.text,
                    IsCorrectChoice = item.IsCorrectChoice


                }).ToList()
            };

            // call generic repo
            var result = await _QuestionRepo.AddAsync(NewQuestion);

            if (result)
                return Ok("Added Sucessfully");
            else
                return NotFound("Not Added Sucessfully");

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _QuestionRepo.DeleteQuestionAndChoicesAsync(id);

            if (result)
                return Ok("Deleted Sucessfully");
            else
                return NotFound("Not Deleted Sucessfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _QuestionRepo.GetAll()
                .Select(ques => new GetAllQuestionDTO
                {
                    id=ques.ID,
                    Title = ques.Title,
                    Choices = ques.Choices.Select(item => new GetAllChoicesDTO
                    {
                        text = item.Text
                    }).ToList()

                }).ToListAsync();

            return Ok(result);
        }

        [HttpPatch]
        public async Task<bool> UpdateQuestion(UpdateQuestionDTO model)
        {

            //mapping
            var newUpdates = new Question
            {
                ID=model.ID,
                Title = model.Title,
            };


            _QuestionRepo.UpdateInclude(newUpdates, nameof(Question.Title));

            return true;
        }

        [HttpPatch]
        public async Task<bool> UpdateChoice(UpdateChoiceDTO model)
        {
            //mappinng

            var ChoiceUpdated = new Choice()
            {
                ID = model.ID,
                Text = model.Text
            };

            //call repo
            _ChoiceRepo.UpdateInclude(ChoiceUpdated, nameof(Choice.Text));
            //return result
            return true;

        }
    }
}
