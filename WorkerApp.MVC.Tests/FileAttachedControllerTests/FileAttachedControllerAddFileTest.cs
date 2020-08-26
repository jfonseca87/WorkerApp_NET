using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorkerApp.Domain;
using WorkerApp.MVC.Controllers;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Tests.FileAttachedControllerTests
{
    [TestClass]
    public class FileAttachedControllerAddFileTest
    {
        [TestMethod]
        public void AddFileDifferentToPhotoCorrectFlow()
        {
            FileAttached file = GetMockRequest();

            var personServiceMock = new Mock<IPersonService>();
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(x => x.GetFile("XXXXX")).Returns(Task.FromResult(GetMockFileTypes()));
            var fileAttachedServiceMock = new Mock<IFileAttachedService>();
            fileAttachedServiceMock.Setup(x => x.AddFile(file.PersonId, file))
                .Returns(Task.FromResult(true));

            NameValueCollection form = new NameValueCollection
            {
                ["FileType"] = file.FileType,
                ["PersonId"] = file.PersonId
            };

            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(x => x.HttpContext.Request.Form).Returns(form);
            mockControllerContext.Setup(x => x.HttpContext.Request.Files.Count).Returns(1);
            mockControllerContext.Setup(x => x.HttpContext.Request.Files[0]).Returns(file.File);

            FileAttachedController fileAttachedController =
                new FileAttachedController(fileAttachedServiceMock.Object, personServiceMock.Object, fileServiceMock.Object)
                {
                    ControllerContext = mockControllerContext.Object
                };

            var result = fileAttachedController.FileAdd().Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("FileAdded"));
        }

        [TestMethod]
        public void AddFileEqualToPhotoCorrectFlow()
        {
            FileAttached file = GetMockImageRequest();

            var fileAttachedServiceMock = new Mock<IFileAttachedService>();
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(x => x.GetFile("XXXXX")).Returns(Task.FromResult(GetMockFileTypes()));
            var personServiceMock = new Mock<IPersonService>();
            personServiceMock.Setup(x => x.GetPerson("ABC123")).Returns(Task.FromResult(GetMockPerson()));

            NameValueCollection form = new NameValueCollection
            {
                ["FileType"] = file.FileType,
                ["PersonId"] = file.PersonId
            };

            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(x => x.HttpContext.Request.Form).Returns(form);
            mockControllerContext.Setup(x => x.HttpContext.Request.Files.Count).Returns(1);
            mockControllerContext.Setup(x => x.HttpContext.Request.Files[0]).Returns(file.File);

            FileAttachedController fileAttachedController =
                new FileAttachedController(fileAttachedServiceMock.Object, personServiceMock.Object, fileServiceMock.Object)
                {
                    ControllerContext = mockControllerContext.Object
                };

            var result = fileAttachedController.FileAdd().Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("PathTest"));
        }

        [TestMethod]
        public void AddFileHasNotAllowedFileExtension()
        {
            FileAttached file = GetMockFileWithNotAllowedFileExntesionRequest();

            var fileAttachedServiceMock = new Mock<IFileAttachedService>();
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(x => x.GetFile("XXXXX")).Returns(Task.FromResult(GetMockFileTypes()));
            var personServiceMock = new Mock<IPersonService>();
            personServiceMock.Setup(x => x.GetPerson("ABC123")).Returns(Task.FromResult(GetMockPerson()));

            NameValueCollection form = new NameValueCollection
            {
                ["FileType"] = file.FileType,
                ["PersonId"] = file.PersonId
            };

            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(x => x.HttpContext.Request.Form).Returns(form);
            mockControllerContext.Setup(x => x.HttpContext.Request.Files.Count).Returns(1);
            mockControllerContext.Setup(x => x.HttpContext.Request.Files[0]).Returns(file.File);

            FileAttachedController fileAttachedController =
                new FileAttachedController(fileAttachedServiceMock.Object, personServiceMock.Object, fileServiceMock.Object)
                {
                    ControllerContext = mockControllerContext.Object
                };

            var result = fileAttachedController.FileAdd().Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        [TestMethod]
        public void AddFileHasNullFileAndFileType()
        {
            var fileAttachedServiceMock = new Mock<IFileAttachedService>();
            var fileServiceMock = new Mock<IFileService>();
            var personServiceMock = new Mock<IPersonService>();

            NameValueCollection form = new NameValueCollection();
            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(x => x.HttpContext.Request.Form).Returns(form);

            FileAttachedController fileAttachedController =
                new FileAttachedController(fileAttachedServiceMock.Object, personServiceMock.Object, fileServiceMock.Object)
                {
                    ControllerContext = mockControllerContext.Object
                };

            fileAttachedController.ModelState.AddModelError(string.Empty, "You must select a file type");
            fileAttachedController.ModelState.AddModelError(string.Empty, "You must select a file");

            var result = fileAttachedController.FileAdd().Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        [TestMethod]
        public void AddFileGenerateException()
        {
            FileAttached file = GetMockFileWithNotAllowedFileExntesionRequest();

            var personServiceMock = new Mock<IPersonService>();
            var fileServiceMock = new Mock<IFileService>();
            var fileAttachedServiceMock = new Mock<IFileAttachedService>();
            fileAttachedServiceMock.Setup(x => x.AddFile(file.PersonId, file))
                .Throws(new Exception());

            NameValueCollection form = new NameValueCollection
            {
                ["FileType"] = file.FileType,
                ["PersonId"] = file.PersonId
            };

            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(x => x.HttpContext.Request.Form).Returns(form);
            mockControllerContext.Setup(x => x.HttpContext.Request.Files.Count).Returns(1);
            mockControllerContext.Setup(x => x.HttpContext.Request.Files[0]).Returns(file.File);

            FileAttachedController fileAttachedController =
                new FileAttachedController(fileAttachedServiceMock.Object, personServiceMock.Object, fileServiceMock.Object)
                {
                    ControllerContext = mockControllerContext.Object
                };

            var result = fileAttachedController.FileAdd().Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.ToString().Contains("Error"));
        }

        private FileAttached GetMockRequest()
        {
            var file = new Mock<HttpPostedFileBase>();
            file.Setup(x => x.FileName).Returns("file1.pdf");

            return new FileAttached()
            {
                File = file.Object,
                FileType = "XXXXX",
                PersonId = Guid.NewGuid().ToString()
            };
        }

        private FileAttached GetMockImageRequest()
        {
            var file = new Mock<HttpPostedFileBase>();
            file.Setup(x => x.FileName).Returns("file1.jpg");

            return new FileAttached()
            {
                File = file.Object,
                FileType = "Photo",
                PersonId = "ABC123"
            };
        }

        private FileAttached GetMockFileWithNotAllowedFileExntesionRequest()
        {
            var file = new Mock<HttpPostedFileBase>();
            file.Setup(x => x.FileName).Returns("file1.exe");

            return new FileAttached()
            {
                File = file.Object,
                FileType = "XXXXX",
                PersonId = "ABC123"
            };
        }

        private File GetMockFileTypes()
        {
            return new File
            {
                FileId = "ABC1234",
                FileType = "XXXXXX",
                AllowedExtensions = ".pdf, .jpg"
            };
        }

        private Person GetMockPerson()
        {
            return new Person
            {
                PersonId = "ABC123",
                Names = "Test",
                Surnames = "Test",
                Photo = "PathTest"
            };
        }
    }
}
