using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models.Orders.Repo;
using WebApi.Models.Repositories;
using WebApi.Models.Orders;
using System.Diagnostics;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/driver")]
    public class DriverController : Controller
    {
        private readonly IUsersRepository usersRepo;
        private readonly IDriverRepository driverRepo;

        public DriverController( IUsersRepository usersRepo, IDriverRepository driverRepo)
        {
            this.usersRepo = usersRepo;
            this.driverRepo = driverRepo;
        }
        [HttpPost("{email}")]
        public async Task<IActionResult> Post(string email, [FromBody]FlavourFrountend[] flavours)
        {
            var result = await driverRepo.UpdateFlavoursPrice(email, flavours);
            return Ok(result);
        }
    }
}