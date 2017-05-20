using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AgeRanger.WebApp.Filters
{
    public class ApiActionFilters : ActionFilterAttribute
    {
        #region Constants

        /// <summary>
        /// The max time allow per action.
        /// </summary>
        private const int MaxTimeAllowPerAction = 3000;

        #endregion

        #region Static Fields

        /// <summary>
        /// The action performance key.
        /// </summary>
        private static readonly string ActionPerformanceKey = "ActionPerformanceKey";

        /// <summary>
        /// Logger for logging.
        /// </summary>
        private readonly ILog logger = LogManager.GetLogger(typeof(ApiActionFilters));

        #endregion

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Model State is Invalid
            if (actionContext.Request.Method == HttpMethod.Post)
            {
                //if (actionContext.ActionArguments["model"] == null)
                //{
                //    actionContext.Response = actionContext.Request
                //        .CreateErrorResponse(HttpStatusCode.NotAcceptable, "Data submit cannot be null");
                //}

                //if (!actionContext.ModelState.IsValid)
                //{
                //    //return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                //    //ModelState.Values.Select(modelState => modelState.Errors).ToList());
                //    actionContext.Response = actionContext.Request
                //    .CreateErrorResponse(HttpStatusCode.NotAcceptable, 
                //    ModelState.Values.Select(modelState => modelState.Errors).ToList());
                //}
            }

            this.logger.InfoFormat(
                "---> [API] Start Executing {0}, action {1}",
                actionContext.ControllerContext.Controller.GetType(),
                actionContext.ActionDescriptor.ActionName);

            // Store current datetime to calculate performance on each action
            actionContext.Request.Properties.Add(
                new KeyValuePair<string, object>(ActionPerformanceKey, DateTime.Now));

            base.OnActionExecuting(actionContext);
        }

        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var startRequest = (DateTime)actionExecutedContext.ActionContext.Request.Properties[ActionPerformanceKey];

            TimeSpan duration = DateTime.Now - startRequest;
            if (duration.Milliseconds > MaxTimeAllowPerAction)
            {
                this.logger.WarnFormat(
                    "---> [API] Process for action [{0}.{1}] take: [{2} ms], it is exceed 3s \r\n",
                    actionExecutedContext.ActionContext.ControllerContext.Controller.GetType(),
                    actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                    duration.Milliseconds);
            }

            this.logger.InfoFormat(
                    "---> [API] End {0}, action {1}. It takes: {2} millisecond ---> \r\n",
                    actionExecutedContext.ActionContext.ControllerContext.Controller.GetType(),
                    actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                    duration.Milliseconds);

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}