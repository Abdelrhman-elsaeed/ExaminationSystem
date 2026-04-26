using ExaminationSystem.DTOs.ExamStudent;
using ExaminationSystem.Helper;
using ExaminationSystem.Models;
using ExaminationSystem.Repo;
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
    }
}
