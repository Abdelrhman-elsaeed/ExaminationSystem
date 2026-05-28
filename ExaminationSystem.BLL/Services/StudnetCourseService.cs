using ExaminationSystem.BLL.DTOs.StudentCourse;
namespace ExaminationSystem.BLL.Services
{
    public class StudnetCourseService
    {
        private readonly GenericRepository<StudentCourse> _StudentCourseRepo;
        private readonly CourseService _CourseService;
        private readonly StudentService _StudentService;
        public StudnetCourseService(GenericRepository<StudentCourse> StudentCourseRepo, StudentService StudentService, CourseService CourseService)
        {
            _StudentCourseRepo = StudentCourseRepo;
            _CourseService = CourseService;
            _StudentService = StudentService;
        }

        public async Task<bool> IsExistAsync(int StudentId,int CourseId)
        {
            return await _StudentCourseRepo.AnyAsync(sc => sc.StudentID==StudentId && sc.CourseID == CourseId && sc.Deleted == false);
        }

        public async Task<bool> IsExistByIdAsync(int id)
        {
            return await _StudentCourseRepo.AnyAsync(sc => sc.ID == id && sc.Deleted == false);
        }

        public async Task<ResponseViewModel<bool>> AssignStudentToCourseAsync(AssignStudentToCourseDTO model)
        {
            // Verify if Student exist
            var isStudentExist = await _StudentService.IsExistAsync(model.StudentID);
            if (!isStudentExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.StudentNotExist, message: "Student not found!");

            // Verify if Course exist
            var isCourseExist = await _CourseService.IsExist(model.CourseID);
            if (!isCourseExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.CourseNotFound, message: "Course not found");

            // Verify if student assigned before to this course (prevent duplication)
            var IsAssignedBefore = await this.IsExistAsync(model.StudentID, model.CourseID);
            if (IsAssignedBefore)
                return ResponseViewModel<bool>.Failure(ErrorCode.StudentAssignedBefore, message: "Student assigned before tho this course");

            var StudentCourseEntity = model.Map<StudentCourse>();
            var result = await _StudentCourseRepo.AddAsync(StudentCourseEntity);

            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.AssignStudentToCoursefail, "Failed to add student to course");

            return ResponseViewModel<bool>.Success(result,message:"Student assigned to course successfully");
        }

        public async Task<ResponseViewModel<bool>> DeleteStudentFromCourseAsync(int id)
        {

            var isExist = await IsExistByIdAsync(id);
            if (!isExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.StudentNotAssignedToCourse, "Student not assigned in this course");

            var result = await _StudentCourseRepo.DeleteAsync(id);
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.DeleteStudentFromCourseFail, "Failed to remove student from course");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Student removed from course successfully");
        }

        //public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateStudentCourseDTO model)
        //{
        //    if (model is null || model.ID <= 0)
        //        return ResponseViewModel<bool>.Failure(ErrorCode.InvalidInput, "Invalid student-course input");

        //    var isExist = await IsExist(model.ID);
        //    if (!isExist)
        //        return ResponseViewModel<bool>.Failure(ErrorCode.NotFound, "Student-course mapping not found");

        //    var updateModel = model.Map<StudentCourse>();

        //    // Adjust the nameof properties based on the actual updateable fields in StudentCourse
        //    var result = await _StudentCourseRepo.UpdateInclude(
        //        updateModel,
        //        nameof(StudentCourse.StudentId),
        //        nameof(StudentCourse.CourseId));

        //    if (!result)
        //        return ResponseViewModel<bool>.Failure(ErrorCode.UpdateFail, "Failed to update student-course mapping");

        //    return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Student-course mapping updated successfully");
        //}
    }
}
