﻿using System.Collections.Generic;
using System.Linq;
using WebApi.Models.Orders;

namespace WebApi.Data
{
    public class OrderDbInitializer
    {
        public static void Initialize(OrderContext context, UserContext userContext)
        {
            context.Database.EnsureCreated();

            if (context.Flavours.Any())
            {
                return;
            }

            var flavours = new List<Flavour>
            {
                new Flavour{ Name = "Vanilla", Price = 1.10 },
                new Flavour{ Name = "Chocolade", Price = 1.20 },
                new Flavour{ Name = "Pistache", Price = 1.30 },
                new Flavour{ Name = "Banana", Price = 1.40 },
                new Flavour{ Name = "Strawberry", Price = 1.50 }

            };

            foreach (var flavour in flavours)
            {
                context.Flavours.Add(flavour);
            }

            context.SaveChanges();

            var drivers = userContext.Drivers.ToList();
            foreach (var driver in drivers)
            {
                foreach (var flavour in flavours)
                {
                    userContext.DriverFlavours.Add(new DriverFlavour
                    {
                        DriverID = driver.DriverID,
                        FlavourID = flavours.Single(f => f.Name == flavour.Name).FlavourID,
                        Price = flavour.Price
                    });
                }
            }
            userContext.SaveChanges();

        }
    }
}
