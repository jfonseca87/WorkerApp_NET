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
    public class PersonControllerPersonTest
    {
        private Mock<IPersonService> mockPersonService;

        [TestInitialize]
        public void InitializeClass()
        {
            mockPersonService = new Mock<IPersonService>();
        }

        [TestMethod]
        public async Task CreatePersonCorrectFlow()
        {
            mockPersonService.Setup(x => x.CreatePerson(It.IsAny<Person>())).ReturnsAsync(GetMockPerson());

            PersonController controller = new PersonController(mockPersonService.Object);

            var result = await controller.Person(GetMockPersonViewModelCreate());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Created"));
        }

        [TestMethod]
        public async Task UpdatePersonCorrectFlow()
        {
            mockPersonService.Setup(x => x.UpdatePerson(It.IsAny<Person>())).Returns(Task.FromResult(GetMockPerson()));

            PersonController controller = new PersonController(mockPersonService.Object);

            var result = await controller.Person(GetMockPersonViewModelUpdate());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Update"));
        }

        [TestMethod]
        public async Task CreatePersonNullProperties()
        {
            mockPersonService.Setup(x => x.CreatePerson(It.IsAny<Person>())).ReturnsAsync(GetMockPerson());

            PersonController controller = new PersonController(mockPersonService.Object);
            controller.ModelState.AddModelError(string.Empty, "You must insert the name");
            controller.ModelState.AddModelError(string.Empty, "You must insert the lastname");

            var result = await controller.Person(GetMockPersonViewModelUpdate());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        [TestMethod]
        public async Task CreatePersonGenerateException()
        {
            mockPersonService.Setup(x => x.CreatePerson(It.IsAny<Person>())).Throws(new Exception());

            PersonController controller = new PersonController(mockPersonService.Object);

            var result = await controller.Person(GetMockPersonViewModelCreate());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        private Person GetMockPerson()
        {
            return new Person
            {
                PersonId = "1",
                Names = "Test",
                Surnames = "Test"
            };
        }

        private PersonViewModel GetMockPersonViewModelCreate()
        {
            return new PersonViewModel
            {
                Names = "Test",
                Surnames = "Test"
            };
        }

        private PersonViewModel GetMockPersonViewModelUpdate()
        {
            return new PersonViewModel
            {
                PersonId = "1",
                Names = "Test",
                Surnames = "Test"
            };
        }
    }
}
