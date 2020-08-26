using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using WorkerApp.Domain;
using WorkerApp.MVC.Models;
using WorkerApp.MVC.Utilities;
using WorkerApp.MVC.Utilities.Validation;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Controllers
{
    [Authorize]
    public class FileAttachedController : Controller
    {
        private readonly IMapper mapper;
        private readonly IFileAttachedService fileAttachedService;
        private readonly IPersonService personService;
        private readonly IFileService fileService;

        public FileAttachedController(IFileAttachedService _fileAttachedService, IPersonService _personService, IFileService _fileService)
        {
            mapper = new MapperSetup().CreateMapperProfile();
            fileAttachedService = _fileAttachedService;
            personService = _personService;
            fileService = _fileService;
        }

        [HttpPost]
        public async Task<JsonResult> FileAdd()
        {
            try
            {
                File fileType;

                FileAttachedInsertViewModel file = new FileAttachedInsertViewModel
                {
                    File = Request.Files?.Count > 0 ? Request.Files[0] : null,
                    FileType = Request.Form.Get("FileType"),
                    PersonId = Request.Form.Get("PersonId")
                };

                TryValidateModel(file);

                if (file.FileType != "Photo" && file.File != null && !string.IsNullOrEmpty(file.FileType)) {
                    fileType = await fileService.GetFile(file.FileType);
                    var fileExtension = file.File.FileName.Substring(file.File.FileName.IndexOf('.'));

                    if (!fileType.AllowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError(string.Empty, "The file doesn`t have an allowed extension");
                    }

                    file.FileType = fileType.FileType;
                }

                if (!ModelState.IsValid)
                {
                    var errors = ProcessValidation.GetValidationErrors(ModelState);
                    return Json(new { Message = "Error", Errors = errors });
                }

                var filedb = mapper.Map<FileAttached>(file);
                await fileAttachedService.AddFile(file.PersonId, filedb);

                if (file.FileType == "Photo")
                {
                    var person = await personService.GetPerson(file.PersonId);
                    return Json(new { Message = person.Photo });
                }

                return Json(new { Message = "FileAdded"});
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> DeleteFile(string personId, string fileId)
        {
            try
            {
                if (string.IsNullOrEmpty(personId))
                {
                    throw new ArgumentNullException(nameof(personId), "The parameter can't be null or empty");
                }

                if (string.IsNullOrEmpty(fileId))
                {
                    throw new ArgumentNullException(nameof(fileId), "The parameter can't be null or empty");
                }

                await fileAttachedService.RemoveFile(personId, fileId);
                return Json(new { Message = "File Deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}