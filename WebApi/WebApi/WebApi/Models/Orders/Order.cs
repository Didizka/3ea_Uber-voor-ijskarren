using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Orders
{
    [Table("Orders")]
    public class Order
    {
        public int OrderID { get; set; }
        public double TotalPrice { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public User Customer { get; set; }
        public Driver Driver { get; set; }
    }
}
