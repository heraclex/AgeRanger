using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AgeRanger.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            this.ConfigLogger();
            this.BootStrapperStart();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        #region private methods

        /// <summary>
        /// Boot strapper start app.
        /// </summary>
        private void BootStrapperStart()
        {
            var bootstrapper = new AutofacBootstrapper();
            bootstrapper.DoStart();
        }

        /// <summary>
        /// Config log 4 net.
        /// </summary>
        private void ConfigLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #endregion
    }
}
