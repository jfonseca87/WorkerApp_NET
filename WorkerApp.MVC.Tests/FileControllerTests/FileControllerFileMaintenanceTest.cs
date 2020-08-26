using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkerApp.Domain;
using WorkerApp.MVC.Controllers;
using WorkerApp.MVC.Models;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Tests.FileControllerTests
{
    [TestClass]
    public class FileControllerFileMaintenanceTest
    {
        [TestMethod]
        public async Task CreateFileCorrectFlow()
        {
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.CreateFile(It.IsAny<File>())).ReturnsAsync(GetMockFile());

            FileController controller = new FileController(mockFileService.Object);

            var result = await controller.MaintenanceFile(GetMockViewCreateFile());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Created"));
        }

        [TestMethod]
        public async Task UpdateFileCorrectFlow()
        {
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.UpdateFile(It.IsAny<File>())).Returns(Task.FromResult(GetMockFile()));

            FileController controller = new FileController(mockFileService.Object);

            var result = await controller.MaintenanceFile(GetMockViewUpdateFile());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Updated"));
        }

        [TestMethod]
        public async Task MaintanceFileFailWithNullParameters()
        {
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.CreateFile(It.IsAny<File>())).ReturnsAsync(GetMockFile());

            FileController controller = new FileController(mockFileService.Object);
            controller.ModelState.AddModelError(string.Empty, "You must insert a file type");
            controller.ModelState.AddModelError(string.Empty, "You must insert allowed extensions");

            var result = await controller.MaintenanceFile(GetMockViewCreateFile());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        [TestMethod]
        public async Task MaintanceFileFailWithException()
        {
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.CreateFile(It.IsAny<File>())).Throws(new Exception());

            FileController controller = new FileController(mockFileService.Object);

            var result = await controller.MaintenanceFile(GetMockViewCreateFile());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        private File GetMockFile()
        {
            return new File 
            { 
                FileId = "1", 
                FileType = "XXXXX", 
                AllowedExtensions = "Test Extension" 
            };  
        }

        private FileViewModel GetMockViewCreateFile()
        {
            return new FileViewModel
            {
                FileType = "XXXXX",
                AllowedExtensions = "Test Extension"
            };
        }

        private FileViewModel GetMockViewUpdateFile()
        {
            return new FileViewModel
            {
                FileId = "1",
                FileType = "XXXXX",
                AllowedExtensions = "Test Extension"
            };
        }
    }
}
