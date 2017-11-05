using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data
{
    public class UserDbInitializer
    {

        public static void Initialize(UserContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }
                        

            var customers = new List<Customer>
            {
                new Customer{ FirstName = "Sanjy", LastName = "Kanagarasa", Password = "test123", RegistrationDate = DateTime.Now, UserRoleType = UserRoleTypes.CUSTOMER, ContactInformation = new ContactInformation { Email = "sanjy-customer@uber.be", PhoneNumber = "111222333444", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Customer{ FirstName = "Chingiz", LastName = "Mizambekov", Password = "test123", RegistrationDate = DateTime.Now, UserRoleType = UserRoleTypes.CUSTOMER, ContactInformation = new ContactInformation { Email = "chingiz-customer@uber.be", PhoneNumber = "4443332221111", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Customer{ FirstName = "Stijn", LastName = "Pittomvils", Password = "test123", RegistrationDate = DateTime.Now, UserRoleType = UserRoleTypes.CUSTOMER, ContactInformation = new ContactInformation { Email = "stijn-customer@uber.be", PhoneNumber = "99999999999", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},

            };

            foreach (var customer in customers)
            {
                context.Customers.Add(customer);
            }

            context.SaveChanges();

            var drivers = new List<Driver>
            {
                new Driver{ FirstName = "Sanjy", LastName = "Kanagarasa", Password = "test123", RegistrationDate = DateTime.Now, UserRoleType = UserRoleTypes.DRIVER, ContactInformation = new ContactInformation { Email = "sanjy-driver@uber.be", PhoneNumber = "111222333444", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Driver{ FirstName = "Chingiz", LastName = "Mizambekov", Password = "test123", RegistrationDate = DateTime.Now, UserRoleType = UserRoleTypes.DRIVER, ContactInformation = new ContactInformation { Email = "chingiz-driver@uber.be", PhoneNumber = "4443332221111", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}},
                new Driver{ FirstName = "Stijn", LastName = "Pittomvils", Password = "test123", RegistrationDate = DateTime.Now, UserRoleType = UserRoleTypes.DRIVER, ContactInformation = new ContactInformation { Email = "stijn-driver@uber.be", PhoneNumber = "99999999999", Address =  new Address { StreetName = "Ellermanstraat", StreetNumber = "41", ZipCode = "2060"}}, IsApproved = true },

            };

            foreach (var driver in drivers)
            {
                context.Drivers.Add(driver);
            }

            context.SaveChanges();



        }


    }
}
