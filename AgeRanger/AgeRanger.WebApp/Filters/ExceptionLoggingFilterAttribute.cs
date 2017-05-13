using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace AgeRanger.WebApp.Filters
{
    public class ExceptionLoggingFilterAttribute : ExceptionFilterAttribute
    {
        protected readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnException(HttpActionExecutedContext context)
        {
            Logger.Error(string.Format("Unhandled exception when processing request {0} {1}.", context.Request.Method, context.Request.RequestUri), context.Exception);
        }
    }
}