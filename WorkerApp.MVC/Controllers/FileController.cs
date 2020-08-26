using System;
using System.Collections.Generic;
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
    public class FileController: Controller
    {
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public FileController(IFileService _fileService)
        {
            mapper = new MapperSetup().CreateMapperProfile();
            fileService = _fileService;
        }

        [Authorize(Roles = Constants.roleAdmin)]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<JsonResult> GetFiles()
        {
            try
            {
                IEnumerable<File> filesDB = await fileService.GetFiles();
                IEnumerable<FileViewModel> filesView = mapper.Map<IEnumerable<FileViewModel>>(filesDB);

                return Json(filesView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public async Task<JsonResult> GetFile(string id)
        {
            try
            {
                var fileDb = await fileService.GetFile(id);
                var fileView = mapper.Map<FileViewModel>(fileDb);

                return Json(fileView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = Constants.roleAdmin)]
        public async Task<JsonResult> MaintenanceFile(FileViewModel file)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    var errors = ProcessValidation.GetValidationErrors(ModelState);
                    return Json(new { Message = "Error", Errors = errors });
                }

                File saveFile = mapper.Map<File>(file);
                File fileDb = new File();
                string message = string.Empty;

                if (!string.IsNullOrEmpty(file.FileId))
                {
                    await fileService.UpdateFile(saveFile);
                    message = "Updated";
                }
                else
                {
                    fileDb = await fileService.CreateFile(saveFile);
                    message = "Created";
                }

                return Json(new { fileDb.FileId, Message = message });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = Constants.roleAdmin)]
        public async Task<JsonResult> DeleteFile(string id)
        {
            try
            {
                var fileDb = await fileService.GetFile(id);

                if (fileDb == null) {
                    return Json(new { Result = "NotFound" });
                }

                await fileService.DeleteFile(id);

                return Json(new { Result = "Success" });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}