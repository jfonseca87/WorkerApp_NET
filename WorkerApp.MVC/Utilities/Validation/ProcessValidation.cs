using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WorkerApp.MVC.Utilities.Validation
{
    public static class ProcessValidation
    {
        public static IEnumerable<string> GetValidationErrors(ModelStateDictionary model)
        {
            List<string> errors = new List<string>();
            var fieldsWithErrors = model.Values.Where(x => x.Errors.Any());

            foreach (var field in fieldsWithErrors)
            {
                foreach (var error in field.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            return errors;
        }
    }
}