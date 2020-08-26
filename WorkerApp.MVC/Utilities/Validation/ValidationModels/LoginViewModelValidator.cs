using FluentValidation;
using WorkerApp.MVC.Models;

namespace WorkerApp.MVC.Utilities.Validation.ValidationModels
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("You must insert a email")
                .EmailAddress().WithMessage("The email doesn't have a valida format");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("You must insert a password");
        }
    }
}