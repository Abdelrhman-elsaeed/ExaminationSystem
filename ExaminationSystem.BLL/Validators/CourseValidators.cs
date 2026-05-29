using ExaminationSystem.BLL.ViewModels.Course;
using ExaminationSystem.BLL.ViewModels.StudentCourse;
using FluentValidation;

namespace ExaminationSystem.BLL.Validators
{
    public class CreateCourseVMValidator : AbstractValidator<CreateCourseVM>
    {
        public CreateCourseVMValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Hours).GreaterThan(0).WithMessage("Course hours must be greater than 0.");
        }
    }

    public class UpdateCourseVMValidator : AbstractValidator<UpdateCourseVM>
    {
        public UpdateCourseVMValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Hours).GreaterThan(0);
        }
    }

    public class AssignStudentToCourseVMValidator : AbstractValidator<AssignStudentToCourseVM>
    {
        public AssignStudentToCourseVMValidator()
        {
            RuleFor(x => x.StudentID).GreaterThan(0);
            RuleFor(x => x.CourseID).GreaterThan(0);
        }
    }
}
