using ExaminationSystem.BLL.DTOs.Course;
using ExaminationSystem.BLL.ViewModels.Course;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services
{
    public class CourseService
    {
        private readonly GenericRepository<Course> _CourseRepo;
        
        public CourseService(GenericRepository<Course> CourseRepo)
        {
            _CourseRepo = CourseRepo;
        }

        public async Task<bool> IsExist(int id)
        {
            return await _CourseRepo.AnyAsync(crs => crs.ID == id);
        }

        public async Task<ResponseViewModel<bool>> AddCourseAsync(CreateCourseDTO model)
        {
            var CourseEntity = model.Map<Course>();

            var IsSaved = await _CourseRepo.AddAsync(CourseEntity);

            if (!IsSaved)
                return ResponseViewModel<bool>.Failure(ErrorCode.AddCourseFail, message: "Fail to add Course!");

            return ResponseViewModel<bool>.Success(true, message: "Course added successfully");
        }

        public async Task<ResponseViewModel<GetCourseDTO>> GetCourseByIdAsync(int id)
        {
            var course = await _CourseRepo.GetByIdAsync(id);

            if (course == null)
                return ResponseViewModel<GetCourseDTO>.Failure(ErrorCode.CourseNotFound, message: "Course not found!");

            var courseVM = course.Map<GetCourseDTO>();

            return ResponseViewModel<GetCourseDTO>.Success(courseVM, message: "Course retrieved successfully");
        }

        public async Task<ResponseViewModel<IEnumerable<GetCourseDTO>>> GetAllCoursesAsync()
        {
            var courses = await _CourseRepo.GetAll().ToListAsync();
            var coursesVM = courses.Map<IEnumerable<GetCourseDTO>>();

            return ResponseViewModel<IEnumerable<GetCourseDTO>>.Success(coursesVM, message: "Courses retrieved successfully");
        }

        public async Task<ResponseViewModel<bool>> UpdateCourseAsync(UpdateCourseDTO model)
        {
            var isExist = await IsExist(model.ID);
            if (!isExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.CourseNotFound, message: "Course not found!");

            var courseEntity = model.Map<Course>();

            var isUpdated = await _CourseRepo.UpdateInclude(courseEntity, nameof(Course.Name), nameof(Course.Description), nameof(Course.Hours));

            if (!isUpdated)
                return ResponseViewModel<bool>.Failure(ErrorCode.UpdateCourseFail, message: "Fail to update Course!");

            return ResponseViewModel<bool>.Success(true, message: "Course updated successfully");
        }

        public async Task<ResponseViewModel<bool>> DeleteAsync(int id)
        {
            var isExist = await IsExist(id);
            if (!isExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.CourseNotFound, message: "Course not found!");

            var isDeleted = await _CourseRepo.DeleteAsync(id);

            if (!isDeleted)
                return ResponseViewModel<bool>.Failure(ErrorCode.DeleteCourseFail, message: "Course not found or fail to delete Course!");

            return ResponseViewModel<bool>.Success(true, message: "Course deleted successfully");
        }
    }
}
