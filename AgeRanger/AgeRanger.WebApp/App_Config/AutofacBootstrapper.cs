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
        /// Interface Controller type.
        /// </summary>
        private static readonly Type IControllerType = typeof(IController);
        
        /// <summary>
        /// Interface Controller type.
        /// </summary>
        private static readonly Type ApiControllerType = typeof(ApiController);

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

            // Set dependence resolver for System.Web.Mvc.DepenceResolver
            //DependencyResolver.SetResolver(new AutofacDependenceResolver(this.container, DependencyResolver.Current));

            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private void Start()
        {
            this.RegisterServiceComponents();
            this.RegisterWebComponents();
            this.ConfigureIoc();
        }

        #region Register For Autofac

        /// <summary>
        ///     Configure IOC container
        /// </summary>
        private void ConfigureIoc()
        {
            this.container = this.builder.Build();
        }

        private void RegisterServiceComponents()
        {
            // Register for Db Context
            var connectionString = ConfigurationManager.ConnectionStrings["LocalDb"].ConnectionString;
            this.builder.RegisterType(typeof(AgeRangerDbContext))
                .WithParameter((pi, c) => pi.ParameterType == typeof(string), (pi, c) => connectionString).AsSelf();
            this.logger.InfoFormat("--- Register {0}", typeof(AgeRangerDbContext).Name);

            // Register for Repository
            this.builder.RegisterGeneric(typeof(AgeRangeRepository<>).GetGenericTypeDefinition())
                    .As(typeof(IAgeRangeRepository<>))
                    .WithParameter(
                        (pi, c) => pi.ParameterType == typeof(AgeRangerDbContext),
                        (pi, c) => c.Resolve<AgeRangerDbContext>()).AsImplementedInterfaces();

            // Register for Service
            builder.RegisterType<AgeRanger.Service.Implementation.AgeRangeService>().As<AgeRanger.Service.Contract.IAgeRangeService>().InstancePerApiRequest();
            this.logger.InfoFormat("--- Register {0}", typeof(AgeRangeRepository<>).Name);
        }

        private void RegisterWebComponents()
        {
            // Register controllers all at once using assembly scanning...
            // Don't need using DI for normal controller
            //this.builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Only apply for API controller
            this.builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var types = this.GetType().Assembly.GetTypes().Where(type => !type.IsAbstract).ToList();            

            //this.builder.RegisterType<AutofacControllerFactory>().As<IControllerFactory>().SingleInstance();
            this.logger.InfoFormat("--- Register {0}", (typeof(AutofacControllerFactory)).Name);
            
            this.logger.Info("Register Web Components is completed");
        }

        #endregion
    }
}