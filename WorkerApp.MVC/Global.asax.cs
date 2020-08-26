using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Mvc;
using WorkerApp.MVC.App_Start;
using WorkerApp.MVC.Utilities.Validation;

namespace WorkerApp.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            IocConfiguration.Configure();
            AreaRegistration.RegisterAllAreas();
            ValidationConfiguration();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void ValidationConfiguration()
        {
            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.ValidatorFactory = new ValidationSetup();
            });
        }
    }
}
