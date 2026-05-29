using ExaminationSystem.BLL.ViewModels.Choice;
using ExaminationSystem.BLL.DTOs.Common; // PaginationParams is still a DTO in common
using ExaminationSystem.BLL.ViewModels.Question;
using FluentValidation;

namespace ExaminationSystem.BLL.Validators
{
    public class CreateChoiceVMValidator : AbstractValidator<CreateChoiceVM>
    {
        public CreateChoiceVMValidator()
        {
            RuleFor(x => x.Text).NotEmpty().MaximumLength(500);
        }
    }

    public class UpdateChoiceVMValidator : AbstractValidator<UpdateChoiceVM>
    {
        public UpdateChoiceVMValidator()
        {
            RuleFor(x => x.ID).GreaterThan(0);
            RuleFor(x => x.Text).NotEmpty().MaximumLength(500);
        }
    }

    public class CreateQuestionVMValidator : AbstractValidator<CreateQuestionVM>
    {
        public CreateQuestionVMValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Level).IsInEnum();
            RuleFor(x => x.CourseId).GreaterThan(0);
            RuleFor(x => x.InstructorId).GreaterThan(0);
            RuleFor(x => x.Choices).NotEmpty().WithMessage("Question must have at least one choice.");
        }
    }

    public class UpdateQuestionVMValidator : AbstractValidator<UpdateQuestionVM>
    {
        public UpdateQuestionVMValidator()
        {
            RuleFor(x => x.ID).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Level).IsInEnum();
        }
    }

    public class RandomQuestionConfigVMValidator : AbstractValidator<RandomQuestionConfigVM>
    {
        public RandomQuestionConfigVMValidator()
        {
            RuleFor(x => x.Count).GreaterThan(0).WithMessage("Question count must be greater than zero.");
            RuleFor(x => x.Level).IsInEnum();
            RuleFor(x => x.GradePerQuestion).GreaterThan(0).WithMessage("Grade per question must be greater than zero.");
        }
    }

    public class PaginationParamsValidator : AbstractValidator<PaginationParams>
    {
        public PaginationParamsValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        }
    }
}
