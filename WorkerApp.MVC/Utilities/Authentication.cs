using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Owin;
using WorkerApp.Domain;

namespace WorkerApp.MVC.Utilities
{
    public static class Authentication
    {
        public static void CreateCookieAuthentication(User user, IOwinContext ctx)
        {
            var identity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("IdUsuario", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Rol)
                },
                "WorkerAppCookie");

            var authManager = ctx.Authentication;
            authManager.SignIn(identity);
        }

        public static void LogOut(IOwinContext ctx)
        {
            var authManager = ctx.Authentication;
            authManager.SignOut("WorkerAppCookie");
        }
    }
}