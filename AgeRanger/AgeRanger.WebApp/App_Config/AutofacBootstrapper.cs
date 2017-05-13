using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Http;
using System.Linq;

using Autofac;
using Autofac.Integration.WebApi;
using log4net;
using AgeRanger.DbContext;
using AgeRanger.Repository;
using Autofac.Integration.Mvc;
using System.Reflection;

namespace AgeRanger.WebApp
{
    /// <summary>
    /// Autofac Bootstrapper Configuration
    /// </summary>
    public class AutofacBootstrapper
    {
        #region Static Fields
        
        /// <summary>
        ///     The log4net logger instance.
        /// </summary>
        private readonly ILog logger = LogManager.GetLogger(typeof(AutofacBootstrapper));

        #endregion

        #region Fields

        /// <summary>
        ///     AUTOFAC builder.
        /// </summary>
        private readonly ContainerBuilder builder = new ContainerBuilder();

        /// <summary>
        ///     The <see cref="IComponentContext" /> from which services
        ///     should be located.
        /// </summary>
        private IContainer container;

        #endregion

        /// <summary>
        ///     Configure IOC container
        /// </summary>
        public void DoStart()
        {
            this.Start();            

            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private void Start()
        {
            this.RegisterComponents();           

            //Configure IOC container
            this.container = this.builder.Build();
        }

        #region Register For Autofac

        private void RegisterComponents()
        {
            // Register for Db Context
            var connectionString = ConfigurationManager.ConnectionStrings["LocalDb"].ConnectionString;
            this.builder.RegisterType(typeof(AgeRangerDbContext))
                .WithParameter((pi, c) => pi.ParameterType == typeof(string), (pi, c) => connectionString).AsSelf();
            this.logger.InfoFormat("Register {0}", typeof(AgeRangerDbContext).Name);

            // Register for Repository
            this.builder.RegisterGeneric(typeof(AgeRangeRepository<>).GetGenericTypeDefinition())
                    .As(typeof(IAgeRangeRepository<>))
                    .WithParameter(
                        (pi, c) => pi.ParameterType == typeof(AgeRangerDbContext),
                        (pi, c) => c.Resolve<AgeRangerDbContext>()).AsImplementedInterfaces();
            this.logger.InfoFormat("Register {0}", typeof(AgeRangeRepository<>).Name);

            // Register for Service
            builder.RegisterType<AgeRanger.Service.Implementation.AgeRangeService>().As<AgeRanger.Service.Contract.IAgeRangeService>().InstancePerApiRequest();
            this.logger.InfoFormat("Register {0}", typeof(AgeRanger.Service.Implementation.AgeRangeService).Name);

            // Only apply for API controller
            this.builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            this.logger.Info("Register API controllers");
        }

        #endregion
    }
}