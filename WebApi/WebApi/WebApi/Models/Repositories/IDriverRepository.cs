using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Orders;
using WebApi.Models.Orders.Resources;

namespace WebApi.Models.Repositories
{
    public interface IDriverRepository
    {
        Task<bool> UpdateFlavoursPrice(string email, FlavourFrountend[] flavours);
        Task<Driver> GetFlavoursPrice(string email);
        Task<List<OrderTotalPriceResource>> CalculatePriceForAllDrivers(ShoppingCart shoppingcart, Order order);
    }
}
