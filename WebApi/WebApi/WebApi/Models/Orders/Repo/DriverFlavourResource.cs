using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Users;

namespace WebApi.Models.Orders.Repo
{
    public class DriverFlavourResource
    {
        public string Email { get; set; }
        public Location Location { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<FlavourResource> Flavours { get; set; }
        public DriverFlavourResource()
        {
            Flavours = new Collection<FlavourResource>();
        }
    }
}
