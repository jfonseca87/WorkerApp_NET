using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace WorkerApp.MVC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "WorkerAppCookie",
                LoginPath = new PathString("/Home/Index"),
                ExpireTimeSpan = System.TimeSpan.FromMinutes(15)
            });
        }
    }
}