using AgeRanger.DbContext;
using AgeRanger.DbContext.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AgeRanger.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        public Repository(AgeRangerDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            this.dbContext = dbContext;
        }

        /// <summary>
        /// The _db context.
        /// </summary>
        private readonly System.Data.Entity.DbContext dbContext;

        #region public methods

        /// <summary>
        /// Get a row by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// An Entity of <see cref="TEntity"/>.
        /// </returns>
        public virtual TEntity Get(long id)
        {
            return this.dbContext.Set<TEntity>().FirstOrDefault(entity => id.Equals(entity.Id));
        }

        /// <summary>
        /// Get a row base on @where condition
        /// </summary>
        /// <param name="where">
        /// The @where condition.
        /// </param>
        /// <returns>
        /// An Entity of <see cref="TEntity"/>.
        /// </returns>
        public virtual TEntity Get(Expression<Func<TEntity, bool>> @where)
        {
            return this.dbContext.Set<TEntity>().Where(@where).FirstOrDefault();
        }

        public ICollection<TEntity> GetAll()
        {
            return this.dbContext.Set<TEntity>().AsQueryable().ToList();
        }

        public ICollection<TEntity> List(Expression<Func<TEntity, bool>> @where)
        {
            return this.dbContext.Set<TEntity>().Where(@where).ToList();
        }

        /// <summary>
        /// Query method will return IQueryable to Repository.
        /// </summary>
        /// <param name="where">
        /// Where condition on Entity.
        /// </param>
        /// <typeparam name="TEntity">
        /// T Entity : conrete class
        /// </typeparam>
        /// <returns>
        /// IQueryable base on T Entity
        /// </returns>
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> @where)
        {
            return this.dbContext.Set<TEntity>().Where(@where).AsQueryable();
        }

        /// <summary>
        /// Save object, delay save until the repository is dispose
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="forceSave">
        /// force Save
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        public virtual TEntity SaveOrUpdate(TEntity entity)
        {
            return this.InternalSaveOrUpdate(entity);
        }

        /// <summary>
        /// Save object immediately to db for further use
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        public virtual TEntity ForceSaveOrUpdateImmediately(TEntity entity)
        {
            return this.InternalSaveOrUpdate(entity, true);
        }     

        /// <summary>
        /// Permanent delete a record in db
        /// </summary>
        /// <param name="id"></param>
        public void DeleteById(long id)
        {
            var entity = this.dbContext.Set(typeof(TEntity)).Find(id);
            if (entity != null)
            {
                this.dbContext.Set<TEntity>().Remove((TEntity)entity);
            }
        }

        #endregion

        #region Dispose methods

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);            
        }

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether or not to dispose managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this.dbContext == null)
                return;

            if (this.dbContext.ChangeTracker.HasChanges() && !this.dbContext.GetValidationErrors().Any())
            {
                this.dbContext.SaveChanges();
            }

            if (this.dbContext.Database.Connection.State == ConnectionState.Open)
            {
                this.dbContext.Database.Connection.Close();
            }
            this.dbContext.Dispose();
        }

        #endregion

        #region private methods

        private string GetEntityName()
        {
            // PluralizationService pluralizer = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));
            // return string.Format("{0}.{1}", ((IObjectContextAdapter)DbContext).ObjectContext.DefaultContainerName, pluralizer.Pluralize(typeof(TEntity).Name));

            // Thanks to Kamyar Paykhan -  http://huyrua.wordpress.com/2011/04/13/entity-framework-4-poco-repository-and-specification-pattern-upgraded-to-ef-4-1/#comment-688
            var entitySetName = ((IObjectContextAdapter)this.dbContext).ObjectContext
                .MetadataWorkspace
                .GetEntityContainer(((IObjectContextAdapter)this.dbContext).ObjectContext.DefaultContainerName, DataSpace.CSpace)
                                    .BaseEntitySets.Where(bes => bes.ElementType.Name == typeof(TEntity).Name).First().Name;
            return string.Format("{0}.{1}", ((IObjectContextAdapter)this.dbContext).ObjectContext.DefaultContainerName, entitySetName);
        }
        
        private TEntity InternalSaveOrUpdate(TEntity entity, bool? forceSave = false)
        {
            if (entity == null)
                return null;
            
            // Add New
            if (entity.Id == 0)
            {
                this.dbContext.Entry(entity).State = System.Data.Entity.EntityState.Added;
                this.dbContext.Set<TEntity>().Add(entity);
            }
            else
            {
                var fqen = this.GetEntityName();

                object originalItem;
                System.Data.Entity.Core.EntityKey key = ((IObjectContextAdapter)this.dbContext).ObjectContext.CreateEntityKey(fqen, entity);
                if (((IObjectContextAdapter)this.dbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
                {
                    ((IObjectContextAdapter)this.dbContext).ObjectContext.ApplyCurrentValues(key.EntitySetName, entity);
                }
            }

            if (forceSave.HasValue && forceSave.Value)
            {
                this.dbContext.SaveChanges();
            }

            return entity;
        }
        
        #endregion

    }
}
