using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.Orders;
using WebApi.Models.Orders.Repo;

namespace WebApi.Models.Repositories
{
    public class DriverRepository: IDriverRepository
    {
        private readonly OrderContext orderContext;
        private readonly IUsersRepository usersRepo;
        private readonly UserContext userContext;

        public DriverRepository(OrderContext orderContext, IUsersRepository usersRepo, UserContext userContext)
        {
            this.orderContext = orderContext;
            this.usersRepo = usersRepo;
            this.userContext = userContext;
        }

        public async Task<Driver> GetFlavoursPrice(string email)
        {
            var driver = await usersRepo.GetDriverByEmail(email);
            if(driver != null)
            {
                return await userContext.Drivers
                    .Include(r => r.DriverFlavours)
                        .ThenInclude(df => df.Flavour)
                    .Include(r => r.ContactInformation)
                    .Include(r => r.Location)
                    .SingleOrDefaultAsync(d => d.DriverID == driver.DriverID);

            }
            return null;
        }

        public async Task<bool> UpdateFlavoursPrice(string email, FlavourFrountend[] flavours)
        {
            var driver = await usersRepo.GetDriverByEmail(email);

            if (flavours == null || driver == null)
            {
                return false;
            }
            List<DriverFlavour> driverFlavours = await usersRepo.GetDriversFlavours(email);
            if (driverFlavours == null || driverFlavours.Count <= 0)
            {
                driverFlavours = new List<DriverFlavour>();
                foreach (var item in flavours)
                {
                    var flavour = orderContext.Flavours.SingleOrDefault(f => f.Name == item.Name);
                    driverFlavours.Add(new DriverFlavour { FlavourID = flavour.FlavourID, DriverID = driver.DriverID, Price = item.Price });
                }
            }
            else
            {
                foreach (var item in flavours)
                {
                    var flavour = orderContext.Flavours.SingleOrDefault(f => f.Name == item.Name);
                    var index = driverFlavours.IndexOf(driverFlavours.SingleOrDefault(df => df.DriverID == driver.DriverID && df.FlavourID == flavour.FlavourID));
                    if (index != -1)
                    {
                        driverFlavours[index].Price = item.Price;
                    }
                }
            }
            driver.DriverFlavours = driverFlavours;
            var result = await userContext.SaveChangesAsync();
            return true;
        }

    }
}
