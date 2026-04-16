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


        [HttpPut]
        public async Task<IActionResult> Add(CreateExamDTO model)
        {

            //validate Course Exist
            var CourseExist = await _CourseRepo.AnyAsync(x=>x.ID==model.CourseId && x.Deleted == false);
            if (!CourseExist)
                return BadRequest("Course is not exist");

            //validate Instructor Exist
            var InstructorExist = await _InstructorRepo.AnyAsync(x=>x.ID==model.InstructorId && x.Deleted == false);
            if (!InstructorExist)
                return BadRequest("Instructor is not exist");

            //mapping
            var newExam = new Exam()
            {
                Name = model.Name,
                Type = model.Type,
                Date = model.Date,
                DurationInMinutes = model.DurationInMinutes,
                InstructorId = model.InstructorId,
                CourseId = model.CourseId
            };

            //calling repo
            var result = await _ExamRepo.AddAsync(newExam);

            if (result)
                return Ok("Exam Added Successfully");
            else
                return NotFound("Exam didn't Added Successfully");

        }

        [HttpPut]
        public async Task<IActionResult> AssignQuestionToExam(AssignQuestionToExamDTO model)
        {

            //1-Validate Exam
            var ExamExist = await _ExamRepo.AnyAsync(x=>x.ID==model.ExamId && x.Deleted == false);
            if (!ExamExist)
                return BadRequest("Exam Not Found");

            //2-Validate Question
            var QuestionExist = await _QuestionRepo.AnyAsync(x => x.ID == model.QuestionId && x.Deleted == false);
            if (!QuestionExist)
                return BadRequest("Question Not Found");

            //3-Validate If Question Assigned To this Exam before (prevent Duplication)
            var IsQuestionExamDuplicated = await _ExamQuestionRepo.AnyAsync(item => item.QuestionId == model.QuestionId && item.ExamId == model.ExamId && item.Deleted == false);
            if (IsQuestionExamDuplicated)
                return BadRequest("This Question Assigned To this Exam before");

            //4-mapping
            var NewExamQuestion = new ExamQuestion()
            {
                ExamId = model.ExamId,
                QuestionId = model.QuestionId,
                Grade = model.Grade
            };

            //5-call repo add
            var result = await _ExamQuestionRepo.AddAsync(NewExamQuestion);

            //6-Validate Add Result
            if (result)
                return Ok("The question has been assigned successfully.");
            else
                return BadRequest("Failed to assign the question. Please try again.");

        }

        [HttpPatch]
        public async Task<IActionResult> Update(UpdateExamDTO model)
        {
            
            //1-Validate Exam
            var ExamExist = await _ExamRepo.AnyAsync(x => x.ID == model.ID && x.Deleted == false);
            if (!ExamExist)
                return BadRequest("Exam Not Found");

            //mapping
            var NewUpdates = new Exam()
            {
                ID=model.ID,
                Name = model.Name,
                Type = model.Type,
                Date = model.Date,
                DurationInMinutes = model.DurationInMinutes
            };

            //call repo
            _ExamRepo.UpdateInclude(NewUpdates, nameof(Exam.Name), nameof(Exam.Type), nameof(Exam.Date), nameof(Exam.DurationInMinutes));

            return Ok("Exam Updated Successfully");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExam(int id)
        {
            //1-Validate Exam
            var ExamExist = await _ExamRepo.AnyAsync(x => x.ID == id && x.Deleted == false);
            if (!ExamExist)
                return BadRequest("Exam Not Found");

            var result = await _ExamRepo.DeleteAsync(id);

            if (result)
                return Ok("Exam Deleted Successfully");
            else
                return BadRequest("Exam failed to delete Successfully");
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateQuestionOnExam(UpdateExamQuestionDTO model)
        {
            //1-Validate Questoin on Exam
            var QuestionOnExamExist = await _ExamQuestionRepo.AnyAsync(x => x.ID == model.ID && x.Deleted==false);
            if (!QuestionOnExamExist)
                return BadRequest("Questoin On Exam Not Found");

            //2-mapping 
            var NewUpdates = new ExamQuestion()
            {
                ID = model.ID,
                Grade = model.Grade
            };

            //3-repo call
            _ExamQuestionRepo.UpdateInclude(NewUpdates, nameof(ExamQuestion.Grade));

            return Ok("Questoin Updated Successfully");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestionFromExam(int id)
        {
            var examQuestionEntry = await _ExamQuestionRepo.AnyAsync(x=>x.ID==id && x.Deleted==false);

            if (!examQuestionEntry)
                return NotFound("Question on Exam not found.");


            var result = await _ExamQuestionRepo.DeleteAsync(id);

            if (result)
                return Ok("Question on Exam deleted Successfully");
            else
                return BadRequest("Question on Exam failed to delete Successfully");
        }

    }
}
