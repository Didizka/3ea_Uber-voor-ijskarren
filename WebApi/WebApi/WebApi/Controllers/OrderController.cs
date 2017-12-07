using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Repositories;
using WebApi.Models.Orders;
using WebApi.Models.Orders.Resources;
using AutoMapper;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepo;
        private readonly IUsersRepository userReop;
        private readonly IDriverRepository driverRepo;
        private readonly IMapper mapper;
        private readonly OrderContext orderContext;

        public OrderController(IOrderRepository orderRepo, 
                                IUsersRepository userReop,
                                IDriverRepository driverRepo,
                                IMapper mapper,
                                OrderContext orderContext)
        {
            this.orderRepo = orderRepo;
            this.userReop = userReop;
            this.driverRepo = driverRepo;
            this.mapper = mapper;
            this.orderContext = orderContext;
        }

        //////////////////////////////////// 
        ///     GET: api/Orders     ////////
        //////////////////////////////////// 
        [HttpGet]
        public async Task<IActionResult> GetAllFlavours()
        {
            var result = await orderRepo.GetFlavoursAsync();

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost("{email}")]
        public async Task<IActionResult> PlaceOrder(string email, [FromBody]ShoppingCart shoppingcart)
        {
            var customer = await userReop.GetCustomerByEmail(email);
            if (shoppingcart == null || customer == null)
            {
                if(shoppingcart==null)
                 return BadRequest(shoppingcart);
                else
                 return BadRequest(customer);
            }

            Order currentOrder = new Order
            {
                CustomerID = customer.CustomerID,
                //Customer = await userContext.Customers.SingleOrDefaultAsync(c => c.CustomerID == customer.CustomerID),
                TotalPrice = 14,
                Location = shoppingcart.Location
            };

            await orderContext.Orders.AddAsync(currentOrder);
            await orderContext.SaveChangesAsync();
            foreach (var order in shoppingcart.Cart)
            {
                //OrderItem orderItem = new OrderItem { Order = await context.Orders.SingleOrDefaultAsync(d => d.OrderID == 1) };
                OrderItem orderItem = new OrderItem { Order = currentOrder };
                orderContext.OrderItems.Add(orderItem);
                List<OrderItemFlavour> orderItemFlavour = new List<OrderItemFlavour>();
                foreach (var item in order.IceCream)
                {
                    var flavour = await orderContext.Flavours.SingleOrDefaultAsync(f => f.Name == item.Name);
                    orderItemFlavour.Add(new OrderItemFlavour { FlavourID = flavour.FlavourID, OrderItem = orderItem, Amount = item.Amount });
                }
                var result = await orderRepo.PlaceOrder(shoppingcart, customer);
                return Ok(result.First().OrderID);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByOrderId(int id)
        {
            var order = await orderRepo.GetOrder(id);
            if(order != null)
            {
                var shoppingCart = mapper.Map<Order, ShoppingCart>(order);
                return Ok(await driverRepo.CalculatePriceForAllDrivers(shoppingCart, order));
            }
                
            return BadRequest("No order with id: " + id);
        }

        [HttpGet("{id}/{driverEmail}")]
        public async Task<IActionResult> GetOrderBackWithPrice(int id, string driverEmail)
        {
            var order = await orderRepo.GetOrder(id);
            var driver = await userReop.GetDriverByEmail(driverEmail);
            if (order != null && driver != null)
            {
                order.Driver = driver;
                return Ok(mapper.Map<Order, OrderResource>(order));
            }

            return BadRequest("No order with id: " + id + ", or can not find driver: " + driverEmail);
        }
        

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
