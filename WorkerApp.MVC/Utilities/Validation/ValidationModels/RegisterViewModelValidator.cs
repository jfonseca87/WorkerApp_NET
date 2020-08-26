using FluentValidation;
using WorkerApp.MVC.Models;

namespace WorkerApp.MVC.Utilities.Validation.ValidationModels
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("You must insert an email")
                .EmailAddress().WithMessage("The email doesn't have a valid format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("You must insert a password")
                .Matches(@"(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[.,;:!·$% &()=?¿@#]).{8,}").WithMessage("The password must have a uppercase letter, a lowercase letter, a number, a special character and minimun length of 8 characters");

            RuleFor(x => x.ValidatePassword)
                .NotEmpty().WithMessage("You must insert the password again")
                .Equal(x => x.Password).WithMessage("The passwords doesn't match");
        }
    }
}