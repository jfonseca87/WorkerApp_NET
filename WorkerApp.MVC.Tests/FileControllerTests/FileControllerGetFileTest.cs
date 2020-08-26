using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkerApp.Domain;
using WorkerApp.MVC.Controllers;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Tests.FileControllerTests
{
    [TestClass]
    public class FileControllerGetFileTest
    {
        [TestMethod]
        public async Task GetFileCorrectFlow()
        {
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetFile("1")).Returns(Task.FromResult(GetMockFile()));

            FileController controller = new FileController(mockFileService.Object);
            var result = await controller.GetFile("1");
            
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public async Task GetFileIncorrectFlow()
        {
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetFile("1")).Throws(new Exception());

            FileController controller = new FileController(mockFileService.Object);
            var result = await controller.GetFile("1");

            Assert.IsNotNull(result);
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
    }
}
