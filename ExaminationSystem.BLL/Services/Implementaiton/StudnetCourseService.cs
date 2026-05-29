using ExaminationSystem.BLL.DTOs.StudentCourse;
using ExaminationSystem.BLL.ViewModels;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.BLL.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Implementaiton
{
    public class StudnetCourseService : IStudnetCourseService
    {
        private readonly IRepository<StudentCourse> _StudentCourseRepo;
        private readonly ICourseService _CourseService;
        private readonly IStudentService _StudentService;
        
        public StudnetCourseService(IRepository<StudentCourse> StudentCourseRepo, IStudentService StudentService, ICourseService CourseService)
        {
            _StudentCourseRepo = StudentCourseRepo;
            _CourseService = CourseService;
            _StudentService = StudentService;
        }

        public async Task<bool> IsExistAsync(int StudentId, int CourseId, CancellationToken cancellationToken = default)
        {
            return await _StudentCourseRepo.CheckExistsByConditionAsync(sc => sc.StudentID == StudentId && sc.CourseID == CourseId && sc.Deleted == false, cancellationToken);
        }

        public async Task<bool> IsExistByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _StudentCourseRepo.CheckExistsByConditionAsync(sc => sc.ID == id && sc.Deleted == false, cancellationToken);
        }

        public async Task<ResponseViewModel<bool>> AssignStudentToCourseAsync(AssignStudentToCourseDTO model, CancellationToken cancellationToken = default)
        {
            var isStudentExist = await _StudentService.IsExistAsync(model.StudentID, cancellationToken);
            if (!isStudentExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.StudentNotExist, message: "Student not found!");

            var isCourseExist = await _CourseService.IsExist(model.CourseID, cancellationToken);
            if (!isCourseExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.CourseNotFound, message: "Course not found");

            var IsAssignedBefore = await this.IsExistAsync(model.StudentID, model.CourseID, cancellationToken);
            if (IsAssignedBefore)
                return ResponseViewModel<bool>.Failure(ErrorCode.StudentAssignedBefore, message: "Student assigned before tho this course");

            var StudentCourseEntity = model.Map<StudentCourse>();
            await _StudentCourseRepo.AddAsync(StudentCourseEntity, cancellationToken);
            var result = await _StudentCourseRepo.SaveChangesAsync(cancellationToken);

            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.AssignStudentToCoursefail, "Failed to add student to course");

            return ResponseViewModel<bool>.Success(result, message: "Student assigned to course successfully");
        }

        public async Task<ResponseViewModel<bool>> DeleteStudentFromCourseAsync(int id, CancellationToken cancellationToken = default)
        {
            var isExist = await IsExistByIdAsync(id, cancellationToken);
            if (!isExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.StudentNotAssignedToCourse, "Student not assigned in this course");

            var sc = await _StudentCourseRepo.GetByIDAsync(id, cancellationToken);
            if (sc == null) return ResponseViewModel<bool>.Failure(ErrorCode.DeleteStudentFromCourseFail, "Failed to remove student from course");

            _StudentCourseRepo.SoftDelete(sc);
            var result = await _StudentCourseRepo.SaveChangesAsync(cancellationToken);
            
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.DeleteStudentFromCourseFail, "Failed to remove student from course");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "Student removed from course successfully");
        }
    }
}
