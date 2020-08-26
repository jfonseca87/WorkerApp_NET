using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkerApp.MVC.Controllers;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Tests.UserControllerTests
{
    [TestClass]
    public class UserControllerLogOutTest
    {
        private Mock<IUserService> mockUserService;
        private Mock<IPersonService> mockPersonService;

        [TestInitialize]
        public void InitializeClass()
        {
            mockUserService = new Mock<IUserService>();
            mockPersonService = new Mock<IPersonService>();
        }

        [TestMethod]
        public void LogOutCorrectFlow()
        {
            UserController controller = new UserController(mockUserService.Object, mockPersonService.Object);
            controller.ControllerContext = GetMockOwinContext(controller);

            var result = controller.Logout();

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void LogOutExceptionFlow()
        {
            UserController controller = new UserController(mockUserService.Object, mockPersonService.Object);

            var result = controller.Logout();

            Assert.IsNotNull(result);
        }

        private ControllerContext GetMockOwinContext(Controller controller)
        {
            Uri url = new Uri("http://locahost/User");
            RouteData routeData = new RouteData();

            HttpRequest httpRequest = new HttpRequest("", url.AbsoluteUri, "");
            HttpResponse httpResponse = new HttpResponse(null);
            HttpContext httpContext = new HttpContext(httpRequest, httpResponse);
            Dictionary<string, object> owinEnvironment = new Dictionary<string, object>()
            {
                {"owin.RequestBody", null}
            };
            httpContext.Items.Add("owin.Environment", owinEnvironment);
            HttpContextWrapper contextWrapper = new HttpContextWrapper(httpContext);

            ControllerContext controllerContext = new ControllerContext(contextWrapper, routeData, controller);

            return controllerContext;
        }
    }
}
