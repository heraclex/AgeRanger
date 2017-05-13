using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Autofac;

namespace AgeRanger.WebApp
{
    /// <summary>
    /// The autofac dependence resolver.
    /// </summary>
    public class AutofacDependenceResolver : IDependencyResolver
    {

        #region Fields

        /// <summary>
        ///     The <see cref="IComponentContext" /> from which services
        ///     should be located.
        /// </summary>
        // private readonly IComponentContext container;
        private readonly IContainer container;

        /// <summary>
        /// The original dependence resolver.
        /// </summary>
        private readonly IDependencyResolver originalResolver;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependenceResolver"/> class.
        /// </summary>
        /// <param name="container">
        /// The <see cref="IComponentContext"/> from which services
        ///     should be located.
        /// </param>
        /// <param name="originalResolver">
        /// The original Resolver.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="container"/> or <paramref name="originalResolver"/> is <see langword="null"/>.
        /// </exception>
        public AutofacDependenceResolver(IContainer container, IDependencyResolver originalResolver)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (originalResolver == null)
            {
                throw new ArgumentNullException("originalResolver");
            }

            this.container = container;
            this.originalResolver = originalResolver;
        }

        #endregion

        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <returns>
        /// The requested service or object.
        /// </returns>
        /// <param name="serviceType">The type of the requested service or object.</param>
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            var ret = this.container.ResolveOptional(serviceType) ?? this.originalResolver.GetService(serviceType);
            return ret;
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <returns>
        /// The requested services.
        /// </returns>
        /// <param name="serviceType">The type of the requested services.</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            Type enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var ret = (IEnumerable<object>)this.container.ResolveOptional(enumerableType)
                      ?? this.originalResolver.GetServices(serviceType);
            return ret;
        }
    }
}