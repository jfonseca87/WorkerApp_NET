using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using WorkerApp.Domain;
using WorkerApp.MVC.Models;
using WorkerApp.MVC.Utilities;
using WorkerApp.MVC.Utilities.Validation;
using WorkerApp.Service.Interfaces;

namespace WorkerApp.MVC.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IPersonService personService;

        public UserController(IUserService _userService, IPersonService _personService)
        {
            mapper = new MapperSetup().CreateMapperProfile();
            userService = _userService;
            personService = _personService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ProcessValidation.GetValidationErrors(ModelState);
                    return Json(new { Message = "Error", Errors = errors });
                }

                User userDb = mapper.Map<User>(user);
                User userAuthenticated = await userService.LogIn(userDb);

                if (userAuthenticated == null) {
                    return Json(new { Message = "UserError" });
                }

                Authentication.CreateCookieAuthentication(userAuthenticated, Request.GetOwinContext());

                if (userAuthenticated.Rol == "Worker")
                {
                    user.Password = string.Empty;
                    user.UserId = userAuthenticated.UserId;

                    Person person = await personService.GetPersonByUserId(userAuthenticated.UserId);

                    return Json(new { Message = "Authenticated", User = user, person?.PersonId });
                }

                return Json(new { Message = "Authenticated", Role = "Admin" });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Register(RegisterViewModel register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ProcessValidation.GetValidationErrors(ModelState);
                    return Json(new { Message = "Error", Errors = errors });
                }

                User user = mapper.Map<User>(register);
                User userCreated = await userService.Register(user);

                Authentication.CreateCookieAuthentication(userCreated, Request.GetOwinContext());

                return Json(new { Message = "Created", User = userCreated });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", behavior: JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Logout() {
            try
            {
                Authentication.LogOut(Request.GetOwinContext());
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return Json("Error", behavior: JsonRequestBehavior.AllowGet);
            }
        }

    }
}