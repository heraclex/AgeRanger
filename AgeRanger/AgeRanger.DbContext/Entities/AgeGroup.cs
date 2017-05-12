using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
namespace AgeRanger.DbContext.Entities
{
    public class AgeGroup : EntityBase
    {
        public long? MinAge { get; set; }

        public long? MaxAge { get; set; }

        public string Description { get; set; }
    }
}
