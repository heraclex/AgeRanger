using Autofac;

using System;
using System.Collections.Concurrent;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Autofac.Core;

namespace AgeRanger.WebApp
{
    /// <summary>
    ///     MVC controller factory based on AUTOFAC.
    /// </summary>
    public class AutofacControllerFactory : IControllerFactory
    {
        #region Static Fields

        /// <summary>
        ///     Logger for logging.
        /// </summary>
        //private static readonly ILogger Log = LogManager.GetLogger(typeof(AutofacControllerFactory));

        /// <summary>
        ///     The session state cache.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, SessionStateBehavior> SessionStateCache =
            new ConcurrentDictionary<Type, SessionStateBehavior>();

        #endregion

        #region Fields

        /// <summary>
        ///     The <see cref="IComponentContext" /> from which services
        ///     should be located.
        /// </summary>
        private readonly IComponentContext container;

        /// <summary>
        ///     Default factory for not registered controller.
        /// </summary>
        private readonly IControllerFactory defaultFactory;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacControllerFactory"/> class. 
        /// Creates a new instance of UnityControllerFactory.
        /// </summary>
        /// <param name="autofacContainer">
        /// The unity container.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The Argument Exception
        /// </exception>
        public AutofacControllerFactory(IComponentContext autofacContainer)
        {
            if (autofacContainer == null)
            {
                throw new ArgumentException();
            }

            this.container = autofacContainer;
            this.defaultFactory = new DefaultControllerFactory();
        }

        #region Public Methods and Operators

        /// <summary>
        /// Retrieves a controller instance for the specified request context and controller type by using Unity.
        /// </summary>
        /// <param name="requestContext">
        /// The request context.
        /// </param>
        /// <param name="controllerType">
        /// The controller type.
        /// </param>
        /// <returns>
        /// The <see cref="IController"/>.
        /// </returns>
        protected IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }

            return this.container.Resolve(controllerType) as IController;
        }

        /// <summary>
        /// Creates the specified controller by using the specified request context.
        /// </summary>
        /// <param name="requestContext">
        /// The request context.
        /// </param>
        /// <param name="controllerName">
        /// The name of the controller.
        /// </param>
        /// <returns>
        /// The controller object.
        /// </returns>
        // [DebuggerStepThrough]
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (this.container.IsRegisteredWithName<IController>(controllerName.ToLowerInvariant()))
            {
                return this.container.ResolveNamed<IController>(controllerName.ToLowerInvariant());
            }

            //Log.ErrorFormat("Unable to find controller with name {0}", controllerName);
            return null;
        }

        /// <summary>
        /// The get controller session behavior.
        /// </summary>
        /// <param name="requestContext">
        /// The request context.
        /// </param>
        /// <param name="controllerName">
        /// The controller name.
        /// </param>
        /// <returns>
        /// The System.Web.SessionState.SessionStateBehavior.
        /// </returns>
        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentNullException("controllerName");
            }

            var keyedService = new KeyedService(controllerName.ToLowerInvariant(), typeof(IController));
            IComponentRegistration registry;
            this.container.ComponentRegistry.TryGetRegistration(keyedService, out registry);
            if (registry != null)
            {
                return this.GetControllerSessionBehavior(requestContext, registry.Activator.LimitType);
            }

            return this.defaultFactory.GetControllerSessionBehavior(requestContext, controllerName);
        }

        /// <summary>
        /// The release controller.
        /// </summary>
        /// <param name="controller">
        /// The controller.
        /// </param>
        public void ReleaseController(IController controller)
        {
            // Do nothing.
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the controller's session behavior.
        /// </summary>
        /// <returns>
        /// The controller's session behavior.
        /// </returns>
        /// <param name="requestContext">
        /// The request context.
        /// </param>
        /// <param name="controllerType">
        /// The type of the controller.
        /// </param>
        protected internal virtual SessionStateBehavior GetControllerSessionBehavior(
            RequestContext requestContext, Type controllerType)
        {
            return SessionStateCache.GetOrAdd(
                controllerType,
                type =>
                {
                    foreach (var attr in controllerType.GetCustomAttributes(typeof(SessionStateAttribute), true))
                    {
                        return ((SessionStateAttribute)attr).Behavior;
                    }

                    return SessionStateBehavior.ReadOnly;
                });
        }

        #endregion
    }
}