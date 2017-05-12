using AgeRanger.DbContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.DbContext.Mappings
{
    public class PersonMap : EntityMappingBase<Person>
    {
        public PersonMap()
        {
            this.ToTable("Person");
            // Properties
            this.Property(t => t.FirstName).IsOptional().HasColumnName("FirstName");
            this.Property(t => t.LastName).IsOptional().HasColumnName("LastName");
            //this.Property(t => t.Age).IsRequired().HasColumnName("Age");
        }
    }
}
