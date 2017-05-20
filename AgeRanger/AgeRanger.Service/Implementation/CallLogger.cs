using Castle.DynamicProxy;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Service.Implementation
{
    public class CallLogger : IInterceptor
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(CallLogger));
        
        public void Intercept(IInvocation invocation)
        {
            this.logger.InfoFormat("Calling method {0} with parameters {1}... ",
              invocation.Method.Name,
              string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            invocation.Proceed();

            this.logger.InfoFormat("Done: result was {0}.", invocation.ReturnValue);
        }
    }
}
