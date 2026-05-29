using ExaminationSystem.BLL.DTOs.Course;
using ExaminationSystem.BLL.ViewModels;
using ExaminationSystem.BLL.ViewModels.Course;
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
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _CourseService;
        private readonly IStudnetCourseService _studnetCourseService;

        public CourseController(ICourseService CourseService, IStudnetCourseService studnetCourseService)
        {
            _CourseService = CourseService;
            _studnetCourseService = studnetCourseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams parameters, CancellationToken cancellationToken)
        {
            var Result = await _CourseService.GetAllCoursesAsync(parameters, cancellationToken);

            if (Result.IsSuccess)
            {
                var mappedResult = new PagedResult<GetCourseVM>
                {
                    Items = Result.Data.Items.Map<IEnumerable<GetCourseVM>>(),
                    TotalCount = Result.Data.TotalCount,
                    PageNumber = Result.Data.PageNumber,
                    PageSize = Result.Data.PageSize
                };
                return Ok(ResponseViewModel<PagedResult<GetCourseVM>>.Success(mappedResult, message: Result.Message));
            }

            return NotFound(ResponseViewModel<PagedResult<GetCourseVM>>.Failure(Result.ErrorCode, message: Result.Message));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _CourseService.GetCourseByIdAsync(id, cancellationToken);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<GetCourseVM>.Success(result.Data.Map<GetCourseVM>(), message: result.Message));

            return NotFound(ResponseViewModel<GetCourseVM>.Failure(result.ErrorCode, message: result.Message));
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Add([FromBody] CreateCourseVM model, CancellationToken cancellationToken)
        {
            var CourseDto =  model.Map<CreateCourseDTO>();
            var result = await _CourseService.AddCourseAsync(CourseDto, cancellationToken);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<GetCourseVM>.Success(result.Data.Map<GetCourseVM>(), message: result.Message));
            
            return BadRequest(ResponseViewModel<GetCourseVM>.Failure(result.ErrorCode, message: result.Message));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _CourseService.DeleteAsync(id, cancellationToken);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<bool>.Success(result.Data, message: result.Message));
            
            return NotFound(ResponseViewModel<bool>.Failure(result.ErrorCode, message: result.Message));
        }

        [HttpPatch]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Update([FromBody] UpdateCourseVM model, CancellationToken cancellationToken)
        {
            var courseDTO = model.Map<UpdateCourseDTO>();
            var result = await _CourseService.UpdateCourseAsync(courseDTO, cancellationToken);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<GetCourseVM>.Success(result.Data.Map<GetCourseVM>(), message: result.Message));
            
            return BadRequest(ResponseViewModel<GetCourseVM>.Failure(result.ErrorCode, message: result.Message));
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AssignStudentToCourse([FromBody] AssignStudentToCourseVM model, CancellationToken cancellationToken)
        {
            var assignDto = model.Map<AssignStudentToCourseDTO>();
            var result = await _studnetCourseService.AssignStudentToCourseAsync(assignDto, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);
            
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteStudentFromCourse(int id, CancellationToken cancellationToken)
        {
            var result = await _studnetCourseService.DeleteStudentFromCourseAsync(id, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);
            
            return NotFound(result);
        }
    }
}
