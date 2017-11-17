using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : Controller
    {
        private readonly OrderContext context;

        public OrderController(OrderContext _context)
        {
            context = _context;
        }

        //////////////////////////////////// 
        ///     GET: api/Orders     ////////
        //////////////////////////////////// 
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await context.Flavours.ToListAsync();

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // GET: api/Order/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Order
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Order/5
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
