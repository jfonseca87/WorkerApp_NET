using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkerApp.Domain;
using WorkerApp.MVC.Controllers;
using WorkerApp.MVC.Models;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Tests.PersonControllerTests
{
    [TestClass]
    public class PersonControllerIndexTest
    {
        private Mock<IPersonService> mockPersonService;

        [TestInitialize]
        public void InitializeClass()
        {
            mockPersonService = new Mock<IPersonService>();
        }

        [TestMethod]
        public async Task IndexCorrectFlow()
        {
            mockPersonService.Setup(x => x.GetPeople()).ReturnsAsync(GetMockPeople());

            PersonController controller = new PersonController(mockPersonService.Object);

            var result = await controller.Index();
            ViewResult view = result as ViewResult;
            IEnumerable<PersonViewModel> people = view.Model as List<PersonViewModel>;

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(3, people.Count());
        }

        [TestMethod]
        public async Task IndexGenerateException()
        {
            mockPersonService.Setup(x => x.GetPeople()).Throws(new Exception());

            PersonController controller = new PersonController(mockPersonService.Object);

            var result = await controller.Index();
            ViewResult view = result as ViewResult;
            string error = view.ViewBag.Error;

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("Error", error);
        }

        private IEnumerable<Person> GetMockPeople()
        {
            return new List<Person>
            {
                new Person { PersonId = "1", Names = "Test", Surnames = "Test" },
                new Person { PersonId = "1", Names = "Test", Surnames = "Test" },
                new Person { PersonId = "1", Names = "Test", Surnames = "Test" }
            };
        }
    }
}
