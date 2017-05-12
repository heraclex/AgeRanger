using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.DbContext.Entities
{
    public class Person : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long? Age { get; set; }
    }
}
