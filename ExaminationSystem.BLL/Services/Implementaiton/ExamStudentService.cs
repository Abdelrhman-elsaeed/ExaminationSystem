using ExaminationSystem.BLL.DTOs.ExamStudent;
using ExaminationSystem.BLL.ViewModels;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.BLL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Implementaiton
{
    public class ExamStudentService : IExamStudentService
    {
        private readonly IRepository<ExamStudent> _ExamStudentRepo;

        public ExamStudentService(IRepository<ExamStudent> examStudentRepo)
        {
            _ExamStudentRepo = examStudentRepo;
        }

        public async Task<bool> AddAsync(CreateExamStudentDTO model, CancellationToken cancellationToken = default)
        {
            var isAssignedBefore = await _ExamStudentRepo.CheckExistsByConditionAsync(x =>
                x.StudentId == model.StudentId &&
                x.ExamId == model.ExamId &&
                x.Deleted == false, cancellationToken);

            if (isAssignedBefore)
                return false;

            var examStudentModel = model.Map<ExamStudent>();
            await _ExamStudentRepo.AddAsync(examStudentModel, cancellationToken);
            return await _ExamStudentRepo.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> SaveFinalGrade(int examId, int studentId, decimal finalGrade, CancellationToken cancellationToken = default)
        {
            var updatedRows = await _ExamStudentRepo
                .GetByCondition(x => x.ExamId == examId && x.StudentId == studentId && x.Deleted == false)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(x => x.FinalGrade, finalGrade), cancellationToken);

            return updatedRows > 0;
        }

        public async Task<bool> IsStudentAssignedToExamAsync(int examId, int studentId, CancellationToken cancellationToken = default)
        {
            return await _ExamStudentRepo.CheckExistsByConditionAsync(x =>
                x.ExamId == examId &&
                x.StudentId == studentId &&
                x.Deleted == false, cancellationToken);
        }

        public async Task<ResponseViewModel<decimal?>> StudentFinalGrade(int StudentId, int ExamId, CancellationToken cancellationToken = default)
        {
            if (StudentId <= 0 || ExamId <= 0)
                return ResponseViewModel<decimal?>.Failure(ErrorCode.InvalidExamInput, "Invalid exam/student input");

            var isRecordExist = await IsStudentAssignedToExamAsync(ExamId, StudentId, cancellationToken);
            if (!isRecordExist)
                return ResponseViewModel<decimal?>.Failure(ErrorCode.StudentNotAssignedToExam, "Student is not assigned to this exam");

            var result = await _ExamStudentRepo
                .GetByCondition(x => x.StudentId == StudentId && x.ExamId == ExamId && !x.Deleted)
                .Select(x => x.FinalGrade)
                .FirstOrDefaultAsync(cancellationToken);

            return ResponseViewModel<decimal?>.Success(result, ErrorCode.None, message: "Final Grade Retrieved Successfully");
        }

        public async Task<ResponseViewModel<IEnumerable<ViewStudentsGradesDTO>>> ViewStudentsGrades(int ExamId, CancellationToken cancellationToken = default)
        {
            if (ExamId <= 0)
                return ResponseViewModel<IEnumerable<ViewStudentsGradesDTO>>.Failure(ErrorCode.InvalidExamInput, "Invalid exam input");

            var result = await _ExamStudentRepo.GetByCondition(es => es.ExamId == ExamId && !es.Deleted)
                .OrderByDescending(es => es.FinalGrade)
                .Select(es => new ViewStudentsGradesDTO()
                {
                    ID = es.ID,
                    StudentId = es.StudentId,
                    ExamId = es.ExamId,
                    StudentName = es.Student.Name,
                    FinalGrade = es.FinalGrade
                }).ToListAsync(cancellationToken);

            return ResponseViewModel<IEnumerable<ViewStudentsGradesDTO>>.Success(result, ErrorCode.None, message: "Final Grade Retrieved Successfully");
        }

        public async Task<ResponseViewModel<decimal?>> TopGrade(int ExamId, CancellationToken cancellationToken = default)
        {
            if (ExamId <= 0)
                return ResponseViewModel<decimal?>.Failure(ErrorCode.InvalidExamInput, "Invalid exam input");

            var result = await _ExamStudentRepo.GetByCondition(es => es.ExamId == ExamId && !es.Deleted && es.FinalGrade.HasValue)
                .OrderByDescending(es => es.FinalGrade)
                .Select(es => es.FinalGrade)
                .FirstOrDefaultAsync(cancellationToken);

            return ResponseViewModel<decimal?>.Success(result, ErrorCode.None, "Top grade retrieved successfully");
        }

        public async Task<ResponseViewModel<decimal?>> AverageGrade(int ExamId, CancellationToken cancellationToken = default)
        {
            if (ExamId <= 0)
                return ResponseViewModel<decimal?>.Failure(ErrorCode.InvalidExamInput, "Invalid exam input");

            var result = await _ExamStudentRepo.GetByCondition(es => es.ExamId == ExamId && !es.Deleted && es.FinalGrade.HasValue)
                .Select(es => es.FinalGrade)
                .AverageAsync(cancellationToken);

            if (result == null)
                return ResponseViewModel<decimal?>.Failure(ErrorCode.StudentNotAssignedToExam, "No graded students found for this exam");

            return ResponseViewModel<decimal?>.Success(result, ErrorCode.None, "Average grade retrieved successfully");
        }
    }
}
