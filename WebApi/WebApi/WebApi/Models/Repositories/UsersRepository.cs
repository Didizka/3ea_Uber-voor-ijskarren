using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.HelperClasses;
using WebApi.Models.Orders;
using WebApi.Models.Users;

namespace WebApi.Models.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserContext context;

        public UsersRepository(UserContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await context.Customers
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .ToListAsync();             
        }
        public async Task<IEnumerable<Driver>> GetDrivers()
        {
            return await context.Drivers
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .ToListAsync();
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            var customer = await context.Customers
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .SingleOrDefaultAsync(u => u.ContactInformation.Email == email);
            //user.Salt = null;
            //user.Password = null;
            return customer;
        }
        public async Task<Driver> GetDriverByEmail(string email)
        {
            var driver = await context.Drivers
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .SingleOrDefaultAsync(u => u.ContactInformation.Email == email);
            //user.Salt = null;
            //user.Password = null;
            return driver;
        }
        public async Task<Customer> GetCustomerById(int id)
        {
            var customer = await context.Customers
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .SingleOrDefaultAsync(u => u.CustomerID == id);
            
            return customer;
        }
        public async Task<Driver> GetDriverById(int id)
        {
            var driver = await context.Drivers
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .SingleOrDefaultAsync(u => u.DriverID == id);

            return driver;
        }

        public async Task<IEnumerable<Driver>> GetDriversLocations()
        {
            var users = await context.Drivers
                                .Include(d => d.Location)
                                .ToListAsync();
            return users;
        }


        public async Task<bool> CanUserLogin(LoginForm loginUser, UserRoleTypes userRole)
        {
            if(userRole == UserRoleTypes.CUSTOMER)
            {
                Customer customer = await GetCustomerByEmail(loginUser.Email);
                string inputPassword = HashedPasswordWithSalt.getHash(loginUser.Password, customer.Salt);
                if (customer.Password == inputPassword)
                    return true;
            }else if(userRole == UserRoleTypes.DRIVER)
            {
                Driver driver = await GetDriverByEmail(loginUser.Email);
                string inputPassword = HashedPasswordWithSalt.getHash(loginUser.Password, driver.Salt);
                if (driver.Password == inputPassword)
                    return true;
            }
            
            return false;
        }
        public async Task<List<DriverFlavour>> GetDriversFlavours(string email)
        {
            Driver driver = await GetDriverByEmail(email);
            List<DriverFlavour> driverFlavours = await context.DriverFlavours.Where(sl => sl.DriverID == driver.DriverID).ToListAsync();
            var users = await context.Drivers
                                .Include(d => d.Location)
                                .ToListAsync();
            return driverFlavours;
        }
        public async Task<UserRoleTypes> CustomerOrDriver(string email)
        {
            var userCustomer = await GetCustomerByEmail(email);
            var userDriver = await GetDriverByEmail(email);
            if (userCustomer != null)
                return UserRoleTypes.CUSTOMER;
            if (userDriver != null)
                return UserRoleTypes.DRIVER;
            return UserRoleTypes.NOTFOUND;

        }
    }
}
