using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.HelperClasses;
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
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await context.Users
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .ToListAsync();             
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await context.Users
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .SingleOrDefaultAsync(u => u.ContactInformation.Email == email);
            //user.Salt = null;
            //user.Password = null;
            return user;
        }
        public async Task<User> GetUserById(int id)
        {
            var user = await context.Users
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .SingleOrDefaultAsync(u => u.UserID == id);
            
            return user;
        }

        public async Task<IEnumerable<Driver>> GetDriversLocations()
        {
            var users = await context.Drivers
                                .Include(d => d.Location)
                                .ToListAsync();
            return users;
        }


        public Boolean CanUserLogin(User user, LoginForm loginUser)
        {
            string inputPassword =  HashedPasswordWithSalt.getHash(loginUser.Password, user.Salt);
            if(user.Password == inputPassword)
            {
                return true;
            }
            return false;
        }
    }
}
