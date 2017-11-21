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

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepo;
        private readonly OrderContext context;
        private readonly UserContext userContext;

        public OrderController(IOrderRepository orderRepo, OrderContext context, UserContext userContext)
        {
            this.orderRepo = orderRepo;
            this.context = context;
            this.userContext = userContext;
        }

        //////////////////////////////////// 
        ///     GET: api/Orders     ////////
        //////////////////////////////////// 
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await orderRepo.GetFlavoursAsync();

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ShoppingCart shoppingcart)
        {
            if (shoppingcart == null)
            {
                return BadRequest();
            }
            Order currentOrder = new Order
            {

                Driver = await userContext.Drivers.SingleOrDefaultAsync(d => d.UserID == 4),
                Customer = await userContext.Customers.SingleOrDefaultAsync(d => d.UserID == 1),
                TotalPrice = 14
            };
            //await context.Orders.AddAsync(currentOrder);
            //await context.SaveChangesAsync();
            foreach (var order in shoppingcart.Cart)
            {
                OrderItem orderItem = new OrderItem { Order = await context.Orders.SingleOrDefaultAsync(d => d.OrderID == 1) };
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
