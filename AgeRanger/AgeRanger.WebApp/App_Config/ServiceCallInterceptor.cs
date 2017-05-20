using Castle.DynamicProxy;
using log4net;
using System;
using System.Diagnostics;
using System.Linq;

namespace AgeRanger.WebApp
{
    
    /// <summary>
    /// Log when call method for performance measurement purpose.
    /// </summary>
    public class ServiceCallInterceptor : IInterceptor
    {
        #region Constants

        /// <summary>
        /// The max time allow per action.
        /// </summary>
        private const int MaxTimeAllowPerServiceMethod = 2000;

        #endregion

        #region Static Fields

        /// <summary>
        ///     The log4net logger instance.
        /// </summary>
        private readonly ILog logger = LogManager.GetLogger(typeof(ServiceCallInterceptor));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Intercept the call
        /// </summary>
        /// <param name="invocation">The invocation object.</param>
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method == null || invocation.Method.DeclaringType == null)
            {
                return;
            }

            string className = invocation.Method.DeclaringType.Name;
            string methodName = invocation.Method.Name;
            this.logger.InfoFormat("[Service] - Call method: {0}.{1} ({2})", 
                className, methodName, string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            // Use StopWatch to calculate performance on each service method
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
                throw;
            }

            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > MaxTimeAllowPerServiceMethod)
            {
                this.logger.WarnFormat(
                    "[Service] - Process for [{0}.{1}] take: [{2} ms], it is exceed 2s",
                    className,
                    methodName,
                    stopwatch.ElapsedMilliseconds);
            }

            this.logger.InfoFormat(
                "[Service] - End call {0}.{1}. It takes: {2} millisecond",
                className,
                methodName,
                stopwatch.ElapsedMilliseconds);
        }

        #endregion
    }
}