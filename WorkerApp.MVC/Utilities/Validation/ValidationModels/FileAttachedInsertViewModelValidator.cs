using FluentValidation;
using WorkerApp.MVC.Models;

namespace WorkerApp.MVC.Utilities.Validation.ValidationModels
{
    public class FileAttachedInsertViewModelValidator : AbstractValidator<FileAttachedInsertViewModel>
    {
        public FileAttachedInsertViewModelValidator()
        {
            RuleFor(x => x.FileType).NotEmpty().WithMessage("You must select a file type");
            RuleFor(x => x.File).NotNull().WithMessage("You must select a file");
        }
    }
}