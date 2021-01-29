using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;

namespace LoginService
{
    public class Global : HttpApplication
    {
        private static Controllers.Controller _verwalter;

        public static Controllers.Controller Verwalter { get => _verwalter; set => _verwalter = value; }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Verwalter = new Controllers.Controller();
        }
    }
}