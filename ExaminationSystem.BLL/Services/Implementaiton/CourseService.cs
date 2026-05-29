using ExaminationSystem.BLL.DTOs.Course;
using ExaminationSystem.BLL.DTOs.Common;
using ExaminationSystem.BLL.ViewModels.Course;
using ExaminationSystem.BLL.Services.Interfaces;
using ExaminationSystem.BLL.ViewModels;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.BLL.AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Implementaiton
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _CourseRepo;
        
        public CourseService(IRepository<Course> CourseRepo)
        {
            _CourseRepo = CourseRepo;
        }

        public async Task<bool> IsExist(int id, CancellationToken cancellationToken = default)
        {
            return await _CourseRepo.CheckExistsByIDAsync(id, cancellationToken);
        }

        public async Task<ResponseViewModel<GetCourseDTO>> AddCourseAsync(CreateCourseDTO model, CancellationToken cancellationToken = default)
        {
            var CourseEntity = model.Map<Course>();

            await _CourseRepo.AddAsync(CourseEntity, cancellationToken);
            var IsSaved = await _CourseRepo.SaveChangesAsync(cancellationToken);

            if (!IsSaved)
                return ResponseViewModel<GetCourseDTO>.Failure(ErrorCode.SaveCourseFail, message: "Fail to add Course!");

            return ResponseViewModel<GetCourseDTO>.Success(CourseEntity.Map<GetCourseDTO>(), message: "Course added successfully");
        }

        public async Task<ResponseViewModel<GetCourseDTO>> GetCourseByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var course = await _CourseRepo.GetByIDAsync(id, cancellationToken);

            if (course == null)
                return ResponseViewModel<GetCourseDTO>.Failure(ErrorCode.CourseNotFound, message: "Course not found!");

            var courseVM = course.Map<GetCourseDTO>();

            return ResponseViewModel<GetCourseDTO>.Success(courseVM, message: "Course retrieved successfully");
        }

        public async Task<ResponseViewModel<PagedResult<GetCourseDTO>>> GetAllCoursesAsync(PaginationParams parameters, CancellationToken cancellationToken = default)
        {
            var query = _CourseRepo.GetAll();

            // Filtering
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                query = query.Where(c => c.Name.Contains(parameters.SearchTerm) || c.Description.Contains(parameters.SearchTerm));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                if (parameters.OrderBy.Equals("name", System.StringComparison.OrdinalIgnoreCase))
                    query = parameters.IsDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name);
                else if (parameters.OrderBy.Equals("hours", System.StringComparison.OrdinalIgnoreCase))
                    query = parameters.IsDescending ? query.OrderByDescending(c => c.Hours) : query.OrderBy(c => c.Hours);
                else
                    query = query.OrderBy(c => c.ID);
            }
            else
            {
                query = query.OrderBy(c => c.ID);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var courses = await query.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                     .Take(parameters.PageSize)
                                     .ToListAsync(cancellationToken);

            var coursesVM = courses.Map<IEnumerable<GetCourseDTO>>();

            var pagedResult = new PagedResult<GetCourseDTO>
            {
                Items = coursesVM,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            return ResponseViewModel<PagedResult<GetCourseDTO>>.Success(pagedResult, message: "Courses retrieved successfully");
        }

        public async Task<ResponseViewModel<GetCourseDTO>> UpdateCourseAsync(UpdateCourseDTO model, CancellationToken cancellationToken = default)
        {
            var isExist = await IsExist(model.ID, cancellationToken);
            if (!isExist)
                return ResponseViewModel<GetCourseDTO>.Failure(ErrorCode.CourseNotFound, message: "Course not found!");

            var courseEntity = model.Map<Course>();

            _CourseRepo.UpdateInclude(courseEntity, nameof(Course.Name), nameof(Course.Description), nameof(Course.Hours));
            var isUpdated = await _CourseRepo.SaveChangesAsync(cancellationToken);

            if (!isUpdated)
                return ResponseViewModel<GetCourseDTO>.Failure(ErrorCode.SaveCourseFail, message: "Fail to update Course!");

            return ResponseViewModel<GetCourseDTO>.Success(courseEntity.Map<GetCourseDTO>(), message: "Course updated successfully");
        }

        public async Task<ResponseViewModel<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var course = await _CourseRepo.GetByIDAsync(id, cancellationToken);
            if (course == null)
                return ResponseViewModel<bool>.Failure(ErrorCode.CourseNotFound, message: "Course not found!");

            _CourseRepo.SoftDelete(course);
            var isDeleted = await _CourseRepo.SaveChangesAsync(cancellationToken);

            if (!isDeleted)
                return ResponseViewModel<bool>.Failure(ErrorCode.SaveCourseFail, message: "Course not found or fail to delete Course!");

            return ResponseViewModel<bool>.Success(true, message: "Course deleted successfully");
        }
    }
}
