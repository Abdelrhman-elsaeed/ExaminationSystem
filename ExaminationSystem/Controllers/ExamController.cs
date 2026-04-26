using ExaminationSystem.ModelDTO.Exam;
using ExaminationSystem.ModelDTO.ExamQuestion;
using ExaminationSystem.Models;
using ExaminationSystem.Repo;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace ExaminationSystem.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class ExamController : ControllerBase
    {
        private readonly GenericRepository<Exam> _ExamRepo;
        private readonly GenericRepository<Course> _CourseRepo;
        private readonly GenericRepository<Question> _QuestionRepo;
        private readonly GenericRepository<ExamQuestion> _ExamQuestionRepo;
        private readonly GenericRepository<Instructor> _InstructorRepo;
        public ExamController(GenericRepository<Exam> ExamRepo, GenericRepository<ExamQuestion> ExamQuestionRepo, GenericRepository<Course> CourseRepo,GenericRepository<Instructor> InstructorRepo, GenericRepository<Question> questionRepo)
        {
            _ExamRepo = ExamRepo;
            _ExamQuestionRepo = ExamQuestionRepo;
            _CourseRepo = CourseRepo;
            _InstructorRepo = InstructorRepo;
            _QuestionRepo = questionRepo;
        }


        //[HttpPut]
        //public async Task<IActionResult> Add(CreateExamDTO model)
        //{



        //}

        //[HttpPut]
        //public async Task<IActionResult> AssignQuestionToExam(AssignQuestionToExamDTO model)
        //{


        //}

        //[HttpPatch]
        //public async Task<IActionResult> Update(UpdateExamDTO model)
        //{
            
           
        //}

        //[HttpDelete]
        //public async Task<IActionResult> DeleteExam(int id)
        //{
           
        //}

        //[HttpPatch]
        //public async Task<IActionResult> UpdateQuestionOnExam(UpdateExamQuestionDTO model)
        //{
            
        //}

        //[HttpDelete]
        //public async Task<IActionResult> DeleteQuestionFromExam(int id)
        //{
            
        //}

    }
}
