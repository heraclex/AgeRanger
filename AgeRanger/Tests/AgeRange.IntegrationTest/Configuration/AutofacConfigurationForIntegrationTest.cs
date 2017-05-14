using AgeRanger.DbContext;
using AgeRanger.Repository;
using Autofac;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRange.IntegrationTest.Configuration
{
    internal class AutofacConfigurationForIntegrationTest
    {
        #region Static Fields

        /// <summary>
        ///     The log4net logger instance.
        /// </summary>
        private readonly ILog logger = LogManager.GetLogger(typeof(AutofacConfigurationForIntegrationTest));

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
        internal IContainer container;

        #endregion
        
        public void DoStart()
        {
            this.RegisterComponents();

            //Configure IOC container
            this.container = this.builder.Build();
        }

        #region Register For Autofac

        private void RegisterComponents()
        {
            // Register for Db Context
            var connectionString = ConfigurationManager.ConnectionStrings["LocalIntegrationTestDb"].ConnectionString;
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
            builder.RegisterType<AgeRanger.Service.Implementation.AgeRangeService>().As<AgeRanger.Service.Contract.IAgeRangeService>();
            this.logger.InfoFormat("Register {0}", typeof(AgeRanger.Service.Implementation.AgeRangeService).Name);            
        }

        #endregion
    }
}
