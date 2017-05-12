using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.DbContext.Entities
{
    public abstract class EntityBase
    {
        public long Id { get; set; }
    }
}
