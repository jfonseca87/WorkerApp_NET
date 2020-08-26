using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkerApp.Domain;
using WorkerApp.MVC.Controllers;
using WorkerApp.MVC.Models;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Tests.PersonControllerTests
{
    [TestClass]
    public class PersonControllerGetPersonTest
    {
        private Mock<IPersonService> mockPersonService;

        [TestInitialize]
        public void InitializeClass()
        {
            mockPersonService = new Mock<IPersonService>();
        }

        [TestMethod]
        public async Task GetPersonCorrectFlow()
        {
            mockPersonService.Setup(x => x.GetPerson(It.IsAny<string>())).ReturnsAsync(GetMockPerson());

            PersonController controller = new PersonController(mockPersonService.Object);

            var result = await controller.GetPerson(It.IsAny<string>());
            PersonViewModel person = result.Data as PersonViewModel;

            Assert.IsNotNull(result.Data);
            Assert.AreEqual("1", person.PersonId);
        }

        [TestMethod]
        public async Task GetPersonNullFlow()
        {
            mockPersonService.Setup(x => x.GetPerson(It.IsAny<string>())).Returns(Task.FromResult<Person>(null));

            PersonController controller = new PersonController(mockPersonService.Object);

            var result = await controller.GetPerson(It.IsAny<string>());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("PersonNotFound"));
        }

        [TestMethod]
        public async Task GetPersonExceptionFlow()
        {
            mockPersonService.Setup(x => x.GetPerson(It.IsAny<string>())).Throws(new Exception());

            PersonController controller = new PersonController(mockPersonService.Object);

            var result = await controller.GetPerson(It.IsAny<string>());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        private Person GetMockPerson()
        {
            return new Person { PersonId = "1", Names = "Test", Surnames = "Test" };
        }
    }
}
