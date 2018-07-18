using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Simple_HMS
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            //if (authCookie != null)
            //{
            //    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            //    var id = 
            //}

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            HMSDependencyResolver.Initialize();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
