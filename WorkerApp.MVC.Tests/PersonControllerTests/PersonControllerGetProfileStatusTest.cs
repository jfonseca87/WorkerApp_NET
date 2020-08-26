using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkerApp.MVC.Controllers;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Tests.PersonControllerTests
{
    [TestClass]
    public class PersonControllerGetProfileStatusTest
    {
        private Mock<IPersonService> mockPersonService;

        [TestInitialize]
        public void InitializeClass()
        {
            mockPersonService = new Mock<IPersonService>();
        }

        [TestMethod]
        public async Task GetyProfileStatusCorrectFlow()
        {
            mockPersonService.Setup(x => x.GetProfileStatus(It.IsAny<string>())).ReturnsAsync(70m);

            PersonController controller = new PersonController(mockPersonService.Object);

            var result = await controller.GetProfileStatus(It.IsAny<string>());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("70"));
        }

        [TestMethod]
        public async Task GetyProfileStatusNullFlow()
        {
            mockPersonService.Setup(x => x.GetProfileStatus(It.IsAny<string>())).Returns(Task.FromResult<decimal?>(null));

            PersonController controller = new PersonController(mockPersonService.Object);

            var result = await controller.GetProfileStatus(It.IsAny<string>());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("PersonNotFound"));
        }

        [TestMethod]
        public async Task GetyProfileStatusExceptionFlow()
        {
            mockPersonService.Setup(x => x.GetProfileStatus(It.IsAny<string>())).Throws(new Exception());

            PersonController controller = new PersonController(mockPersonService.Object);

            var result = await controller.GetProfileStatus(It.IsAny<string>());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }
    }
}
