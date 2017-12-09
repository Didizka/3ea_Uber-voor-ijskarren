using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.Orders;
using WebApi.Models.Orders.Resources;

namespace WebApi.Models.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly OrderContext context;
        private readonly IDriverRepository driverRepo;
        private readonly IUsersRepository userRepo;

        public OrderRepository(OrderContext context, IDriverRepository driverRepo, IUsersRepository userRepo)
        {
            this.context = context;
            this.driverRepo = driverRepo;
            this.userRepo = userRepo;
        }

        public async Task<IEnumerable<Flavour>> GetFlavoursAsync()
        {
            return await context.Flavours.ToListAsync();
        }

        public async Task<IEnumerable<OrderTotalPriceResource>> PlaceOrder(ShoppingCart shoppingcart, Customer customer)
        {

            Order currentOrder = new Order
            {
                CustomerID = customer.CustomerID,
                Location = shoppingcart.Location,
                Status = OrderStatus.Pending
            };
            await context.Orders.AddAsync(currentOrder);
            await context.SaveChangesAsync();
            foreach (var order in shoppingcart.Cart)
            {
                OrderItem orderItem = new OrderItem { Order = currentOrder };
                context.OrderItems.Add(orderItem);
                List<OrderItemFlavour> orderItemFlavour = new List<OrderItemFlavour>();
                foreach (var item in order.IceCream)
                {
                    var flavour = await context.Flavours.SingleOrDefaultAsync(f => f.Name == item.Name);
                    orderItemFlavour.Add(new OrderItemFlavour { FlavourID = flavour.FlavourID, OrderItem = orderItem, Amount = item.Amount });
                }

                orderItem.OrderItemFlavours = orderItemFlavour;
                await context.OrderItems.AddAsync(orderItem);
                await context.SaveChangesAsync();
            }
            return await driverRepo.CalculatePriceForAllDrivers(shoppingcart, currentOrder); ;
        }
        public async Task<bool> ConfirmOrder(Order order, ConfirmOrderResource confirmOrder)
        {
            var driver = await userRepo.GetDriverByEmail(confirmOrder.DriverEmail);
            if(driver != null)
            {
                order.DriverID = driver.DriverID;
                order.TotalPrice = confirmOrder.TotalPrice;
                order.Status = OrderStatus.Confirmed;
                order.Payed = true;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<Order> GetOrder(int orderId)
        {
            return await context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(o => o.OrderItemFlavours)
                        .ThenInclude(o => o.Flavour)
                 .SingleOrDefaultAsync(o => o.OrderID == orderId);
        }
        public async Task<ShoppingCart> GetOrderBack(Order order)
        {
            return null;
        }



    }
}
