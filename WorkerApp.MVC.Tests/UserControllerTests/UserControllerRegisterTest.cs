using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkerApp.Domain;
using WorkerApp.MVC.Controllers;
using WorkerApp.MVC.Models;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Tests.UserControllerTests
{
    [TestClass]
    class UserControllerRegisterTest
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
        public async Task RegisterCorrectFlow()
        {
            mockUserService.Setup(x => x.Register(It.IsAny<User>())).ReturnsAsync(GetMockUser());

            UserController controller = new UserController(mockUserService.Object, mockPersonService.Object);

            var result = await controller.Register(GetMockRegisterViewModel());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Created"));
        }

        [TestMethod]
        public async Task RegisterNullPropertiesFlow()
        {
            mockUserService.Setup(x => x.Register(It.IsAny<User>())).ReturnsAsync(GetMockUser());

            UserController controller = new UserController(mockUserService.Object, mockPersonService.Object);
            controller.ModelState.AddModelError(string.Empty, "You must insert a valid email");

            var result = await controller.Register(GetMockRegisterViewModel());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Created"));
        }

        [TestMethod]
        public async Task RegisterExceptionFlow()
        {
            mockUserService.Setup(x => x.Register(It.IsAny<User>())).Throws(new Exception());

            UserController controller = new UserController(mockUserService.Object, mockPersonService.Object);

            var result = await controller.Register(GetMockRegisterViewModel());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        private RegisterViewModel GetMockRegisterViewModel()
        {
            return new RegisterViewModel()
            {
                Email = "test",
                Password = "abc123",
                ValidatePassword = "abc123"
            };
        }

        private User GetMockUser()
        {
            return new User
            {
                UserId = "1",
                Email = "Test",
                Rol = "Admin"
            };
        }
    }
}
