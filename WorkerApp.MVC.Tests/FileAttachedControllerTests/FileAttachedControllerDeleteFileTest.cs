using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkerApp.MVC.Controllers;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Tests.FileAttachedControllerTest
{
    [TestClass]
    public class FileAttachedControllerDeleteFileTest
    {
        [TestMethod]
        public void DeleteFileCorrectFlow()
        {
            // Services Mocks
            var personServiceMock = new Mock<IPersonService>();
            var fileServiceMock = new Mock<IFileService>();
            string personId = Guid.NewGuid().ToString();
            string fileId = Guid.NewGuid().ToString();

            var fileAttachedServiceMock = new Mock<IFileAttachedService>();
            fileAttachedServiceMock.Setup(x => x.RemoveFile(personId, fileId))
                                    .Returns(Task.FromResult(true));

            FileAttachedController fileAttachedController =
                new FileAttachedController(fileAttachedServiceMock.Object, personServiceMock.Object, fileServiceMock.Object);
            var result = fileAttachedController.DeleteFile(personId, fileId).Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("File Deleted"));
        }

        [TestMethod]
        public void DeleteFileIncorrectFlow()
        {
            // Services Mocks
            var personServiceMock = new Mock<IPersonService>();
            var fileServiceMock = new Mock<IFileService>();
            string personId = Guid.NewGuid().ToString();
            string fileId = Guid.NewGuid().ToString();

            var fileAttachedServiceMock = new Mock<IFileAttachedService>();
            fileAttachedServiceMock.Setup(x => x.RemoveFile(personId, fileId))
                                    .Throws(new Exception());

            FileAttachedController fileAttachedController =
                new FileAttachedController(fileAttachedServiceMock.Object, personServiceMock.Object, fileServiceMock.Object);
            var result = fileAttachedController.DeleteFile(personId, fileId).Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        [TestMethod]
        public void DeleteFileNullPersonIdParameter()
        {
            // Services Mocks
            var personServiceMock = new Mock<IPersonService>();
            var fileServiceMock = new Mock<IFileService>();
            string personId = null;
            string fileId = Guid.NewGuid().ToString();

            var fileAttachedServiceMock = new Mock<IFileAttachedService>();
            fileAttachedServiceMock.Setup(x => x.RemoveFile(personId, fileId))
                                    .Throws(new Exception());

            FileAttachedController fileAttachedController =
                new FileAttachedController(fileAttachedServiceMock.Object, personServiceMock.Object, fileServiceMock.Object);
            var result = fileAttachedController.DeleteFile(personId, fileId).Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        [TestMethod]
        public void DeleteFileNullFileIdParameter()
        {
            // Services Mocks
            var personServiceMock = new Mock<IPersonService>();
            var fileServiceMock = new Mock<IFileService>();
            string personId = Guid.NewGuid().ToString();
            string fileId = null;

            var fileAttachedServiceMock = new Mock<IFileAttachedService>();
            fileAttachedServiceMock.Setup(x => x.RemoveFile(personId, fileId))
                                    .Throws(new Exception());

            FileAttachedController fileAttachedController =
                new FileAttachedController(fileAttachedServiceMock.Object, personServiceMock.Object, fileServiceMock.Object);
            var result = fileAttachedController.DeleteFile(personId, fileId).Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }
    }
}
