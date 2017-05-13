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
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.Age).HasColumnName("Age");
        }
    }
}
