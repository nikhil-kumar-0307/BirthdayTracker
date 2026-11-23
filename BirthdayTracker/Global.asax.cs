using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace BirthdayTracker
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value)) return;

            try
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket == null || ticket.Expired) return;

                var identity = new FormsIdentity(ticket);
                var roles = (ticket.UserData ?? "").Split(',');

                HttpContext.Current.User = new GenericPrincipal(identity, roles);
            }
            catch
            {
                // invalid cookie: ignore, user will be asked to log in again
            }
        }
    }
}
