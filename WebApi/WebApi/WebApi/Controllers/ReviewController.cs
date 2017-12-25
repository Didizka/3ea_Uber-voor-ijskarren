using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Review;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Review")]
    public class ReviewController : Controller
    {
        // GET: api/Review
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Review/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Review
        [HttpPost("driver/{email}")]
        public async Task<IActionResult> PostDriverReview(string email, [FromBody]DriverReview driverRiview)
        {
            return Ok( driverRiview);
        }
        
        // PUT: api/Review/5
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
