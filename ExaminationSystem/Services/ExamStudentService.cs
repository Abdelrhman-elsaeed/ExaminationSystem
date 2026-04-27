using ExaminationSystem.DTOs.ExamStudent;
using ExaminationSystem.Enums;
using ExaminationSystem.Helper;
using ExaminationSystem.Models;
using ExaminationSystem.Repo;
using ExaminationSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Services
{
    public class ExamStudentService
    {
        private readonly GenericRepository<ExamStudent> _ExamStudentRepo;

        public ExamStudentService(GenericRepository<ExamStudent> examStudentRepo)
        {
            _ExamStudentRepo = examStudentRepo;
        }

        public async Task<bool> AddAsync(CreateExamStudentDTO model)
        {
            var isAssignedBefore = await _ExamStudentRepo.AnyAsync(x =>
                x.StudentId == model.StudentId &&
                x.ExamId == model.ExamId &&
                x.Deleted == false);

            if (isAssignedBefore)
                return false;

            var examStudentModel = model.Map<ExamStudent>();
            return await _ExamStudentRepo.AddAsync(examStudentModel);
        }

        public async Task<bool> SaveFinalGrade(int examId, int studentId, decimal finalGrade)
        {
            var updatedRows = await _ExamStudentRepo
                .Get(x => x.ExamId == examId && x.StudentId == studentId && x.Deleted == false)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(x => x.FinalGrade, finalGrade));

            return updatedRows > 0;
        }

        public async Task<bool> IsStudentAssignedToExamAsync(int examId, int studentId)
        {
            return await _ExamStudentRepo.AnyAsync(x =>
                x.ExamId == examId &&
                x.StudentId == studentId &&
                x.Deleted == false);
        }

        public async Task<ResponseViewModel<decimal?>> StudentFinalGrade(int StudentId, int ExamId)
        {
            if(StudentId <= 0 || ExamId <= 0)
                return ResponseViewModel<decimal?>.Failure(ErrorCode.InvalidExamInput, "Invalid exam/student input");

            var isRecordExist = await IsStudentAssignedToExamAsync(ExamId, StudentId);
            if (!isRecordExist)
                return ResponseViewModel<decimal?>.Failure(ErrorCode.StudentNotAssignedToExam, "Student is not assigned to this exam");

            var result = await _ExamStudentRepo
                .Get(x => x.StudentId == StudentId && x.ExamId == ExamId && !x.Deleted)
                .Select(x => x.FinalGrade)
                .FirstOrDefaultAsync();

            return ResponseViewModel<decimal?>.Success(result, ErrorCode.None, message: "Final Grade Retrieved Successfully");
        }

        public async Task<ResponseViewModel<IEnumerable<ViewStudentsGradesDTO> >> ViewStudentsGrades(int ExamId)
        {
            if (ExamId <= 0)
                return ResponseViewModel<IEnumerable<ViewStudentsGradesDTO>>.Failure(ErrorCode.InvalidExamInput, "Invalid exam input");

            var result = await _ExamStudentRepo.Get(es => es.ExamId == ExamId && !es.Deleted)
                .OrderByDescending(es=>es.FinalGrade)
                .Select(es => new ViewStudentsGradesDTO()
                {
                    ID = es.ID,
                    StudentId = es.StudentId,
                    ExamId = es.ExamId,
                    StudentName = es.Student.Name,
                    FinalGrade = es.FinalGrade

                }).ToListAsync();

            return ResponseViewModel<IEnumerable<ViewStudentsGradesDTO>>.Success(result, ErrorCode.None, message: "Final Grade Retrieved Successfully");
        }

        public async Task<ResponseViewModel<decimal?>> TopGrade(int ExamId)
        {
            if (ExamId <= 0)
                return ResponseViewModel<decimal?>.Failure(ErrorCode.InvalidExamInput, "Invalid exam input");

            var result = await _ExamStudentRepo.Get(es => es.ExamId == ExamId && !es.Deleted && es.FinalGrade.HasValue)
                .OrderByDescending(es => es.FinalGrade)
                .Select(es => es.FinalGrade)
                .FirstOrDefaultAsync();

            return ResponseViewModel<decimal?>.Success(result, ErrorCode.None, "Top grade retrieved successfully");
        }

        public async Task<ResponseViewModel<decimal?>> AverageGrade(int ExamId)
        {
            if (ExamId <= 0)
                return ResponseViewModel<decimal?>.Failure(ErrorCode.InvalidExamInput, "Invalid exam input");

            var result = await _ExamStudentRepo.Get(es => es.ExamId == ExamId && !es.Deleted && es.FinalGrade.HasValue)
                .Select(es=>es.FinalGrade)
                .AverageAsync();


            if (result == null)
                return ResponseViewModel<decimal?>.Failure(ErrorCode.StudentNotAssignedToExam, "No graded students found for this exam");

            return ResponseViewModel<decimal?>.Success(result, ErrorCode.None, "Average grade retrieved successfully");
        }
    }
}
