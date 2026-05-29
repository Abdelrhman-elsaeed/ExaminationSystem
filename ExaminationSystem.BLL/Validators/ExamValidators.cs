using System;
using ExaminationSystem.BLL.ViewModels.Exam;
using ExaminationSystem.BLL.ViewModels.ExamQuestion;
using ExaminationSystem.BLL.ViewModels.ExamStudent;
using FluentValidation;

namespace ExaminationSystem.BLL.Validators
{
    public class CreateExamVMValidator : AbstractValidator<CreateExamVM>
    {
        public CreateExamVMValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.Date).GreaterThanOrEqualTo(DateTime.Today).WithMessage("Exam date cannot be in the past.");
            RuleFor(x => x.DurationInMinutes).GreaterThan(0).WithMessage("Duration must be greater than 0 minutes.");
            RuleFor(x => x.InstructorId).GreaterThan(0);
            RuleFor(x => x.CourseId).GreaterThan(0);
        }
    }

    public class CreateRandomExamVMValidator : AbstractValidator<CreateRandomExamVM>
    {
        public CreateRandomExamVMValidator()
        {
            RuleFor(x => x.CourseId).GreaterThan(0);
            RuleFor(x => x.ExamId).GreaterThan(0);
            RuleFor(x => x.QuestionsConfig).NotEmpty().WithMessage("You must specify at least one question configuration.");
        }
    }

    public class UpdateExamVMValidator : AbstractValidator<UpdateExamVM>
    {
        public UpdateExamVMValidator()
        {
            RuleFor(x => x.ID).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.Date).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(x => x.DurationInMinutes).GreaterThan(0);
        }
    }

    public class SubmitExamVMValidator : AbstractValidator<SubmitExamVM>
    {
        public SubmitExamVMValidator()
        {
            RuleFor(x => x.ExamId).GreaterThan(0);
            RuleFor(x => x.StudentId).GreaterThan(0);
            RuleFor(x => x.Answers).NotNull().WithMessage("Answers list cannot be null.");
        }
    }

    public class CreateExamStudentVMValidator : AbstractValidator<CreateExamStudentVM>
    {
        public CreateExamStudentVMValidator()
        {
            RuleFor(x => x.ExamId).GreaterThan(0);
            RuleFor(x => x.StudentId).GreaterThan(0);
        }
    }

    public class AssignQuestionToExamVMValidator : AbstractValidator<AssignQuestionToExamVM>
    {
        public AssignQuestionToExamVMValidator()
        {
            RuleFor(x => x.ExamId).GreaterThan(0);
            RuleFor(x => x.QuestionId).GreaterThan(0);
            RuleFor(x => x.Grade).GreaterThan(0).WithMessage("Grade must be greater than zero.");
        }
    }

    public class UpdateExamQuestionVMValidator : AbstractValidator<UpdateExamQuestionVM>
    {
        public UpdateExamQuestionVMValidator()
        {
            RuleFor(x => x.ID).GreaterThan(0);
            RuleFor(x => x.Grade).GreaterThan(0);
        }
    }
}
