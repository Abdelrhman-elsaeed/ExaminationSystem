using ExaminationSystem.BLL.DTOs.Course;
using ExaminationSystem.BLL.ViewModels.Course;


namespace ExaminationSystem.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class CourseController : ControllerBase
    {

        private readonly CourseService _CourseService;

        public CourseController(CourseService CourseService)
        {
            _CourseService = CourseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Result = await _CourseService.GetAllCoursesAsync();

            var AllCoursesDataVM = Result.Data.Map<IEnumerable<GetCourseVM>>();

            if (Result.IsSuccess)
                return Ok(ResponseViewModel<IEnumerable<GetCourseVM>>.Success(AllCoursesDataVM, message: Result.Message));
            else
                return NotFound(ResponseViewModel<IEnumerable<GetCourseVM>>.Failure(Result.ErrorCode, message: Result.Message));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _CourseService.GetCourseByIdAsync(id);

            if (result.IsSuccess)
            {
                var courseVM = result.Data.Map<GetCourseVM>();
                return Ok(ResponseViewModel<GetCourseVM>.Success(courseVM, message: result.Message));
            }

            return NotFound(ResponseViewModel<GetCourseVM>.Failure(result.ErrorCode, message: result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCourseVM model)
        {

            var CourseDto =  model.Map<CreateCourseDTO>();
            var result = await _CourseService.AddCourseAsync(CourseDto);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<bool>.Success(result.Data, message: result.Message));
            
            return BadRequest(ResponseViewModel<bool>.Failure(result.ErrorCode, message: result.Message));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _CourseService.DeleteAsync(id);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<bool>.Success(result.Data, message: result.Message));
            
            return NotFound(ResponseViewModel<bool>.Failure(result.ErrorCode, message: result.Message));
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateCourseVM model)
        {
            var courseDTO = model.Map<UpdateCourseDTO>();
            var result = await _CourseService.UpdateCourseAsync(courseDTO);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<bool>.Success(result.Data, message: result.Message));
            
            return BadRequest(ResponseViewModel<bool>.Failure(result.ErrorCode, message: result.Message));
        }

    }
}
