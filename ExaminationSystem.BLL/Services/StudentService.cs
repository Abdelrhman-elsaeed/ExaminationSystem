using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.BLL.Services
{
    public class StudentService
    {
        private readonly GenericRepository<Student> _StudentRepo;

        public StudentService(GenericRepository<Student> StudentRepo)
        {
            _StudentRepo = StudentRepo;
        }

        public async Task<bool> IsExistAsync(int id)
        {
            return await _StudentRepo.AnyAsync(crs => crs.ID == id);
        }
    }
}
