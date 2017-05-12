using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.DbContext.Mappings
{
    public abstract class EntityMappingBase<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : Entities.EntityBase
    {
        protected EntityMappingBase()
        {
            this.HasKey(x => x.Id);
        }
    }
}
