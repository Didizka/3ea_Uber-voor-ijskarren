using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Orders;
using WebApi.Models.Users;

namespace WebApi.Models.Repositories
{
    public interface IUsersRepository
    {
        Task<Customer> GetCustomerByEmail(string email);
        Task<Driver> GetDriverByEmail(string email);
        Task<Customer> GetCustomerById(int id);
        Task<Driver> GetDriverById(int id);
        Task<UserRoleTypes> CustomerOrDriver(string email);
        Task<bool> CanUserLogin(LoginForm loginUser, UserRoleTypes userRole);
        Task<IEnumerable<Customer>> GetCustomers();
        Task<IEnumerable<Driver>> GetDrivers();
        Task<IEnumerable<Driver>> GetDriversLocations();
        Task<List<DriverFlavour>> GetDriversFlavours(string email);
        void RemovePasswordOfCustomer(Customer customer);
        void RemovePasswordOfDriver(Driver customer);
        Task<bool> CreateDriverFlavourTable(Driver driver);
    }
}