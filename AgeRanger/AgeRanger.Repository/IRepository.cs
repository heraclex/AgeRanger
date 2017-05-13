using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Repository
{
    /// <summary>
    /// Provides a standard interface for Repositories which are data-access mechanism agnostic.
    ///     Since nearly all of the domain objects you create will have a type of EntityBase TEntity, this
    ///     base IRepository assumes that. 
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// Returns null if a row is not found matching the provided Id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        TEntity Get(long id);

        /// <summary>
        /// Returns null if a row is not found matching the provided Id.
        /// </summary>
        /// <param name="where">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        TEntity Get(Expression<Func<TEntity, bool>> @where);

        /// <summary>
        /// Returns all T instances.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        ICollection<TEntity> GetAll();

        /// <summary>
        /// Returns List of rows found matching with condition
        /// </summary>
        /// <param name="where">
        /// Query Condition
        /// </param>
        /// <returns>
        /// The List of <see cref="TEntity"/>.
        /// </returns>
        ICollection<TEntity> List(Expression<Func<TEntity, bool>> @where);

        /// <summary>
        /// Query method will return IQueryable to Repository.
        /// </summary>
        /// <param name="where">
        /// Where condition on Entity.
        /// </param>
        /// <typeparam name="T">
        /// T Entity : conrete class
        /// </typeparam>
        /// <returns>
        /// IQueryable base on T Entity
        /// </returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> @where);

        /// <summary>
        /// SaveOrUpdate
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        TEntity SaveOrUpdate(TEntity entity);

        /// <summary>
        /// SaveOrUpdate
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        TEntity ForceSaveOrUpdateImmediately(TEntity entity);
                
        void DeleteById(long id);
    }
}
