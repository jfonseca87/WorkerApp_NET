using FluentValidation;
using WorkerApp.MVC.Models;

namespace WorkerApp.MVC.Utilities.Validation.ValidationModels
{
    public class FileViewModelValidator : AbstractValidator<FileViewModel>
    {
        public FileViewModelValidator()
        {
            RuleFor(x => x.FileType).NotEmpty().WithMessage("You must insert a file type");
            RuleFor(x => x.AllowedExtensions).NotEmpty().WithMessage("You must insert allowed extensions");
        }
    }
}