using ExaminationSystem.BLL.ViewModels.Auth;
using ExaminationSystem.BLL.ViewModels.User;
using FluentValidation;

namespace ExaminationSystem.BLL.Validators
{
    public class RegisterVMValidator : AbstractValidator<RegisterVM>
    {
        public RegisterVMValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }

    public class TokenRequestVMValidator : AbstractValidator<TokenRequestVM>
    {
        public TokenRequestVMValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class AddRoleVMValidator : AbstractValidator<AddRoleVM>
    {
        public AddRoleVMValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Role).IsInEnum();
        }
    }

    public class AddUserVMValidator : AbstractValidator<AddUserVM>
    {
        public AddUserVMValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Role).IsInEnum();
        }
    }

    public class UpdateUserVMValidator : AbstractValidator<UpdateUserVM>
    {
        public UpdateUserVMValidator()
        {
            RuleFor(x => x.ID).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Role).IsInEnum();
        }
    }
}
