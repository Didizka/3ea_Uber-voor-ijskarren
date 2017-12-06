using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Orders;
using WebApi.Models.Orders.Resources;

namespace WebApi.Models.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Flavour>> GetFlavoursAsync();
        Task<IEnumerable<OrderTotalPriceResource>> PlaceOrder(ShoppingCart shoppingcart, Customer customer);
        Task<Order> GetOrder(int orderId);
    }
}