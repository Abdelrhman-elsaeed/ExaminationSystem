namespace ExaminationSystem.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _QuestionService;
        public QuestionController(QuestionService QuestionService)
        {
            _QuestionService = QuestionService;
        }

        [HttpPut]
        public async Task<ActionResult> Add(CreateQuestionVM model)
        {
            var newQuestionDto = model.Map<CreateQuestionDTO>();
            var result = await _QuestionService.AddAsync(newQuestionDto);

            if (result.Data)
                return Ok(result);
            else
                return NotFound(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _QuestionService.DeleteQuestionAndChoicesAsync(id);
            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var ResultDTO = await _QuestionService.GetAllAsync();

            if (!ResultDTO.IsSuccess)
            {
                return NotFound(ResultDTO);
            }

            var ResultVM = ResultDTO.Data.Select(q => q.Map<GetAllQuestionVM>()).ToList();

            return Ok(ResponseViewModel<List<GetAllQuestionVM>>.Success(ResultVM, message: ResultDTO.Message));

        }

        [HttpPatch]
        public async Task<IActionResult> UpdateQuestion(UpdateQuestionVM model)
        {

            var UpdatesDTO = model.Map<UpdateQuestionDTO>();

            var result = await _QuestionService.UpdateQuestionAsync(UpdatesDTO);

            if (result.IsSuccess)
                return Ok(result);
            else
                return NotFound(result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateChoice(UpdateChoiceVM model)
        {
            var UpdateDTO = model.Map<UpdateChoiceDTO>();

            var result = await _QuestionService.UpdateChoiceAsync(UpdateDTO);

            if (result.IsSuccess)
                return Ok(result);
            else
                return NotFound(result);
        }

    }
}
