using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkerApp.Domain;
using WorkerApp.MVC.Controllers;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Tests.FileControllerTests
{
    [TestClass]
    public class FileControllerGetFilesTest
    {
        [TestMethod]
        public async Task GetFilesCorrectFlow()
        {
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetFiles()).Returns(Task.FromResult(GetMockFiles()));

            FileController controller = new FileController(mockFileService.Object);

            var result = await controller.GetFiles();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetFilesIncorrectFlow()
        {
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetFiles()).Throws(new Exception());

            FileController controller = new FileController(mockFileService.Object);

            var result = await controller.GetFiles();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        private IEnumerable<File> GetMockFiles()
        {
            return new List<File>
            {
                new File { FileId = "1", FileType = "XXXXX", AllowedExtensions = "Test Extension" },
                new File { FileId = "1", FileType = "XXXXX", AllowedExtensions = "Test Extension" }
            };
        }
    }
}
