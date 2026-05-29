using ExaminationSystem.BLL.Services.Interfaces;


namespace ExaminationSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Instructor")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _QuestionService;

        public QuestionController(IQuestionService QuestionService)
        {
            _QuestionService = QuestionService;
        }

        [HttpPut]
        public async Task<ActionResult> Add(CreateQuestionVM model, CancellationToken cancellationToken)
        {
            var newQuestionDto = model.Map<CreateQuestionDTO>();
            var result = await _QuestionService.AddAsync(newQuestionDto, cancellationToken);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<GetAllQuestionVM>.Success(result.Data.Map<GetAllQuestionVM>(), message: result.Message));

            return NotFound(ResponseViewModel<GetAllQuestionVM>.Failure(result.ErrorCode, message: result.Message));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _QuestionService.DeleteQuestionAndChoicesAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams parameters, CancellationToken cancellationToken)
        {
            var ResultDTO = await _QuestionService.GetAllAsync(parameters, cancellationToken);

            if (!ResultDTO.IsSuccess)
                return NotFound(ResultDTO);
            
            var ResultVM = new PagedResult<GetAllQuestionVM>
            {
                Items = ResultDTO.Data.Items.Select(q => q.Map<GetAllQuestionVM>()).ToList(),
                TotalCount = ResultDTO.Data.TotalCount,
                PageNumber = ResultDTO.Data.PageNumber,
                PageSize = ResultDTO.Data.PageSize
            };

            return Ok(ResponseViewModel<PagedResult<GetAllQuestionVM>>.Success(ResultVM, message: ResultDTO.Message));
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateQuestion(UpdateQuestionVM model, CancellationToken cancellationToken)
        {
            var UpdatesDTO = model.Map<UpdateQuestionDTO>();

            var result = await _QuestionService.UpdateQuestionAsync(UpdatesDTO, cancellationToken);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<GetAllQuestionVM>.Success(result.Data.Map<GetAllQuestionVM>(), message: result.Message));

            return NotFound(ResponseViewModel<GetAllQuestionVM>.Failure(result.ErrorCode, message: result.Message));
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateChoice(UpdateChoiceVM model, CancellationToken cancellationToken)
        {
            var UpdateDTO = model.Map<UpdateChoiceDTO>();

            var result = await _QuestionService.UpdateChoiceAsync(UpdateDTO, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);

            return NotFound(result);
        }
    }
}
