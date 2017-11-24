using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Orders;

namespace WebApi.Models
{
    [Table("Drivers")]
    public class Driver : User
    {
        [DefaultValue(false)]
        public bool IsApproved { get; set; }

        public ICollection<DriverFlavour> DriverFlavours { get; set; }

        public Driver()
        {
            DriverFlavours = new Collection<DriverFlavour>();
        }
        // to be continued
    }
}
