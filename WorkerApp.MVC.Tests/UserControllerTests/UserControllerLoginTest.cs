using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkerApp.Domain;
using WorkerApp.MVC.Controllers;
using WorkerApp.MVC.Models;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Tests.UserControllerTests
{
    [TestClass]
    public class UserControllerLoginTest
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
        public async Task LoginAdminCorrectFlow()
        {
            mockUserService.Setup(x => x.LogIn(It.IsAny<User>())).ReturnsAsync(GetMockUserAdmin());

            UserController controller = new UserController(mockUserService.Object, mockPersonService.Object);
            controller.ControllerContext = GetMockOwinContext(controller);

            var result = await controller.Login(GetMockUserViewModel());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Admin"));
        }

        [TestMethod]
        public async Task LoginWorkerCorrectFlow()
        {
            mockUserService.Setup(x => x.LogIn(It.IsAny<User>())).ReturnsAsync(GetMockUserWorker());
            mockPersonService.Setup(x => x.GetPersonByUserId(It.IsAny<string>())).ReturnsAsync(GetMockPerson());

            UserController controller = new UserController(mockUserService.Object, mockPersonService.Object);
            controller.ControllerContext = GetMockOwinContext(controller);

            var result = await controller.Login(GetMockUserViewModel());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Worker"));
        }

        [TestMethod]
        public async Task LoginWorkerNullParametersFlow()
        {
            mockUserService.Setup(x => x.LogIn(It.IsAny<User>())).ReturnsAsync(GetMockUserWorker());

            UserController controller = new UserController(mockUserService.Object, mockPersonService.Object);
            controller.ModelState.AddModelError(string.Empty, "You must insert a email");
            controller.ModelState.AddModelError(string.Empty, "You must insert a password");

            var result = await controller.Login(GetMockUserViewModel());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        [TestMethod]
        public async Task LoginNullUserFlow()
        {
            mockUserService.Setup(x => x.LogIn(It.IsAny<User>())).Returns(Task.FromResult<User>(null));

            UserController controller = new UserController(mockUserService.Object, mockPersonService.Object);

            var result = await controller.Login(GetMockUserViewModel());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("UserError"));
        }

        [TestMethod]
        public async Task LoginExceptionFlow()
        {
            mockUserService.Setup(x => x.LogIn(It.IsAny<User>())).Throws(new Exception());

            UserController controller = new UserController(mockUserService.Object, mockPersonService.Object);

            var result = await controller.Login(GetMockUserViewModel());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        private LoginViewModel GetMockUserViewModel()
        {
            return new LoginViewModel
            {
                Email = "test",
                Password = "abc123"
            };
        }

        private User GetMockUserAdmin()
        {
            return new User
            {
                UserId = "1",
                Email = "Test",
                Rol = "Admin"
            };
        }

        private User GetMockUserWorker()
        {
            return new User
            {
                UserId = "1",
                Email = "Test",
                Rol = "Worker"
            };
        }

        private Person GetMockPerson()
        {
            return new Person { PersonId = "1", Names = "Test", Surnames = "Test" };
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
