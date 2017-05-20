
using AgeRanger.DbContext.Entities;
using AgeRanger.Repository;
using Antlr.Runtime.Misc;
using Castle.DynamicProxy;
using log4net;
using System;
using System.Linq;

namespace AgeRanger.WebApp
{
    public class RepositoryCallInterceptor : IInterceptor
    {
        #region Static Fields

        /// <summary>
        ///     The log4net logger instance.
        /// </summary>
        private readonly ILog logger = LogManager.GetLogger(typeof(RepositoryCallInterceptor));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Intercept the service method call. This method will be wrapper in transaction management
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
            this.logger.InfoFormat("[Repository] - Call method: {0}.{1}({2})",
                className, methodName, string.Join(", ", invocation.Arguments.Select(a => (a ?? string.Empty).ToString()).ToArray()));

            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
                throw;
            }

            this.logger.InfoFormat(
                "[Repository] - End call {0}.{1}.",
                className,
                methodName);
        }

        #endregion              
    }
}