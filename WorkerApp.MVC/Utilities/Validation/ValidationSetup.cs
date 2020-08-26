using System;
using System.Collections.Generic;
using FluentValidation;
using WorkerApp.MVC.Models;
using WorkerApp.MVC.Utilities.Validation.ValidationModels;

namespace WorkerApp.MVC.Utilities.Validation
{
    public class ValidationSetup : ValidatorFactoryBase
    {
        private static readonly Dictionary<Type, IValidator> validators = new Dictionary<Type, IValidator>();

        static ValidationSetup() => RegisterValidators();

        public override IValidator CreateInstance(Type validatorType)
        {
            if (validators.TryGetValue(validatorType, out IValidator validator))
            {
                return validator;
            }

            return validator;
        }

        private static void RegisterValidators()
        {
            validators.Add(typeof(IValidator<LoginViewModel>), new LoginViewModelValidator());
            validators.Add(typeof(IValidator<FileViewModel>), new FileViewModelValidator());
            validators.Add(typeof(IValidator<RegisterViewModel>), new RegisterViewModelValidator());
            validators.Add(typeof(IValidator<PersonViewModel>), new PersonViewModelValidator());
            validators.Add(typeof(IValidator<FileAttachedInsertViewModel>), new FileAttachedInsertViewModelValidator());
        }
    }
}