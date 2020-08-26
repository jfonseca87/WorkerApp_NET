using FluentValidation;
using WorkerApp.MVC.Models;

namespace WorkerApp.MVC.Utilities.Validation.ValidationModels
{
    public class PersonViewModelValidator : AbstractValidator<PersonViewModel>
    {
        public PersonViewModelValidator()
        {
            RuleFor(x => x.Names)
                .NotEmpty().WithMessage("You must insert names")
                .Matches(@"^[a-zA-Zñ\s]+$").WithMessage("This field cannot have numeric or special characters");

            RuleFor(x => x.Surnames)
                .NotEmpty().WithMessage("You must insert surnames")
                .Matches(@"^[a-zA-Zñ\s]+$").WithMessage("This field cannot have numeric or special characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("You must insert a email")
                .EmailAddress().WithMessage("The email must be a correct format");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("You must insert a phone number");
            RuleFor(x => x.Profession).NotEmpty().WithMessage("You must insert a profession");
        }
    }
}