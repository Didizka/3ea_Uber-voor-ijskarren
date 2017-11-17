using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Orders
{
    [Table("OrderItemFlavours")]
    public class OrderItemFlavour
    {
        public int OrderItemID { get; set; }
        public int FlavourID { get; set; }

        public Flavour Flavour { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}
