using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Service.Contract.Models
{
    /// <summary>
    /// Data Transfer Object AgeGroup. THis object will will transfer to client side
    /// </summary>
    public class AgeGroupModel
    {
        public long Id { get; set; }

        public long MinAge { get; set; }

        public long MaxAge { get; set; }

        public string Description { get; set; }
    }
}
