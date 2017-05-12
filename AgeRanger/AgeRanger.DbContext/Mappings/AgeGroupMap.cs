using AgeRanger.DbContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.DbContext.Mappings
{
    public class AgeGroupMap : EntityMappingBase<AgeGroup>
    {
        public AgeGroupMap()
        {
            this.ToTable("AgeGroup");

            // Properties
            //this.Property(t => t.MinAge).IsOptional().HasColumnName("MinAge");
            //this.Property(t => t.MaxAge).IsOptional().HasColumnName("MaxAge");
            this.Property(t => t.Description).IsRequired().HasColumnName("Description");
        }
    }
}
