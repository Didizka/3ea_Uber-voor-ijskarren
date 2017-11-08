using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Users;

namespace WebApi.Models.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(int id);
        Boolean CanUserLogin(User user, LoginForm loginUser);
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<Driver>> GetDriversLocations();
    }
}