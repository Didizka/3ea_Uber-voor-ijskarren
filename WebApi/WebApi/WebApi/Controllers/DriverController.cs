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
        private readonly OrderContext orderContext;
        private readonly UserContext userContext;
        private readonly IUsersRepository usersRepo;

        public DriverController(OrderContext orderContext, UserContext userContext, IUsersRepository usersRepo)
        {
            this.orderContext = orderContext;
            this.userContext = userContext;
            this.usersRepo = usersRepo;
        }
        [HttpPost("{email}")]
        public async Task<IActionResult> Post(string email, [FromBody]FlavourFrountend[] flavours)
        {
            var driver = await usersRepo.GetDriverByEmail(email);

            if (flavours == null || driver == null)
            {
                return BadRequest(driver);
            }
            List<DriverFlavour> driverFlavours = userContext.DriverFlavours.Where(sl => sl.UserID == driver.UserID).ToList();
            Debug.WriteLine(driverFlavours);
            if (driverFlavours == null || driverFlavours.Count <= 0)
            {
                driverFlavours = new List<DriverFlavour>();
                foreach (var item in flavours)
                {
                    var flavour = orderContext.Flavours.SingleOrDefault(f => f.Name == item.Name);
                    driverFlavours.Add(new DriverFlavour { FlavourID = flavour.FlavourID, UserID = driver.UserID, Price = item.Price });
                }
            }
            else
            {
                foreach (var item in flavours)
                {
                    var flavour = orderContext.Flavours.SingleOrDefault(f => f.Name == item.Name);
                    var index = driverFlavours.IndexOf(driverFlavours.SingleOrDefault(df => df.UserID == driver.UserID && df.FlavourID == flavour.FlavourID));
                    if (index != -1)
                    {
                        driverFlavours[index].Price = item.Price;
                    }
                }
            }
            driver.DriverFlavours = driverFlavours;
            await userContext.SaveChangesAsync();
            return Ok(true);
        }
    }
}