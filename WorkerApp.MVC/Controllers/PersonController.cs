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
    [Authorize]
    public class PersonController : Controller
    {
        private readonly IMapper mapper;
        private readonly IPersonService personService;

        public PersonController(IPersonService _personService)
        {
            mapper = new MapperSetup().CreateMapperProfile();
            personService = _personService;
        }

        [Authorize(Roles = Constants.roleAdmin)]
        public async Task<ActionResult> Index()
        {
            try
            {
                IEnumerable<Person> workersDb = await personService.GetPeople();
                IEnumerable<PersonViewModel> workers = mapper.Map<List<PersonViewModel>>(workersDb);

                return View(workers);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                ViewBag.Error = "Error";

                return View();
            }
        }

        public ActionResult Person()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Person(PersonViewModel personView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ProcessValidation.GetValidationErrors(ModelState);
                    return Json(new { Message = "Error", Errors = errors });
                }

                string personId = string.Empty;
                var personDb = mapper.Map<Person>(personView);

                string message;
                if (personView.PersonId is null)
                {
                    var personResult = await personService.CreatePerson(personDb);
                    personId = personResult.PersonId;
                    message = "Created";
                }
                else
                {
                    await personService.UpdatePerson(personDb);
                    message = "Updated";
                }

                return Json(new { PersonId = personId, Message = message });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetPerson(string id) 
        {
            try
            {
                var personDB = await personService.GetPerson(id);
                var personView = mapper.Map<PersonViewModel>(personDB);

                if (personDB == null) 
                {
                    return Json(new { Message = "PersonNotFound" }, behavior: JsonRequestBehavior.AllowGet);
                }

                return Json(personView, behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetProfileStatus(string id) 
        {
            try
            {
                decimal? status = await personService.GetProfileStatus(id);
                if (status.HasValue)
                {
                    return Json(new { Message = status }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = "PersonNotFound" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", behavior: JsonRequestBehavior.AllowGet);
            }
        }
    }
}