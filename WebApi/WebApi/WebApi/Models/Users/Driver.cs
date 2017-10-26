using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    [Table("Drivers")]
    public class Driver : User
    {
        [DefaultValue(false)]
        public bool IsApproved { get; set; }

        // to be continued
    }
}
