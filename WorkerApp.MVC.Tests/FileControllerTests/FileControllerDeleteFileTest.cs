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
    public class FileControllerDeleteFileTest
    {
        Mock<IFileService> mockFileService;

        [TestInitialize]
        public void InitializeClassTest()
        {
            mockFileService = new Mock<IFileService>();
        }

        [TestMethod]
        public async Task DeleteFileCorrectFlow()
        {
            mockFileService.Setup(x => x.DeleteFile(It.IsAny<string>())).Returns(Task.FromResult(true));
            mockFileService.Setup(x => x.GetFile(It.IsAny<string>())).ReturnsAsync(GetMockFile());

            FileController controller = new FileController(mockFileService.Object);

            var result = await controller.DeleteFile(It.IsAny<string>());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("Success"));
        }

        [TestMethod]
        public async Task DeleteFileNotFound()
        {
            mockFileService.Setup(x => x.DeleteFile(It.IsAny<string>())).Returns(Task.FromResult(false));
            mockFileService.Setup(x => x.GetFile(It.IsAny<string>())).Returns(Task.FromResult<File>(null));

            FileController controller = new FileController(mockFileService.Object);

            var result = await controller.DeleteFile(It.IsAny<string>());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("NotFound"));
        }

        [TestMethod]
        public async Task DeleteFileGenerateException()
        {
            mockFileService.Setup(x => x.DeleteFile(It.IsAny<string>())).Throws(new Exception());
            mockFileService.Setup(x => x.GetFile(It.IsAny<string>())).ReturnsAsync(GetMockFile());

            FileController controller = new FileController(mockFileService.Object);

            var result = await controller.DeleteFile(It.IsAny<string>());

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
