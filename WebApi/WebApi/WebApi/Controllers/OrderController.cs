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
using WebApi.Models.Orders.Resources;
using AutoMapper;
using WebApi.Models;

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
        public async Task<IActionResult> GetAllUsers()
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

            var result = await orderRepo.PlaceOrder(shoppingcart, customer);
            return Ok(result.First().OrderID);
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
