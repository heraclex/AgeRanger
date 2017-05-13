using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Service.Contract.Models
{
    /// <summary>
    /// Data Transfer Object Person. THis object will transfer to client side
    /// </summary>
    public  class PersonModel
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]        
        public string LastName { get; set; }

        [Required]
        [Range(0, long.MaxValue)]
        public long? Age { get; set; }

        public string AgeGroup { get; set; }
    }
}
