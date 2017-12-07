using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Repositories;
using WebApi.Models.Orders;
using WebApi.Models.Orders.Repo;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepo;
        private readonly IUsersRepository userReop;
        private readonly OrderContext context;
        private readonly UserContext userContext;

        public OrderController(IOrderRepository orderRepo, IUsersRepository userReop, OrderContext context, UserContext userContext)
        {
            this.orderRepo = orderRepo;
            this.userReop = userReop;
            this.context = context;
            this.userContext = userContext;
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
        public async Task<IActionResult> PostNewOrder(string email, [FromBody]ShoppingCart shoppingcart)
        {
            var customer = await userReop.GetCustomerByEmail(email);
            if (shoppingcart == null || customer == null)
            {
                return BadRequest();
            }
            Order currentOrder = new Order
            {
                CustomerID = customer.CustomerID,
                //Customer = await userContext.Customers.SingleOrDefaultAsync(c => c.CustomerID == customer.CustomerID),
                TotalPrice = 14,
                Location = shoppingcart.Location
            };
            await context.Orders.AddAsync(currentOrder);
            await context.SaveChangesAsync();
            foreach (var order in shoppingcart.Cart)
            {
                //OrderItem orderItem = new OrderItem { Order = await context.Orders.SingleOrDefaultAsync(d => d.OrderID == 1) };
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
            //throw new Exception();
            return Ok(true);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
