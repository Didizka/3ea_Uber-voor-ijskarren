using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Users;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using WebApi.Models.Repositories;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly UserContext context;
        private readonly IMapper mapper;
        private readonly IUsersRepository usersRepo;

        public UsersController(UserContext _context, IMapper _mapper, IUsersRepository usersRepo) {
            context = _context;
            mapper = _mapper;
            this.usersRepo = usersRepo;
        }

        //////////////////////////////////// 
        ///     GET: api/Users      ////////
        //////////////////////////////////// 
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await usersRepo.GetUsers();
                    
            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        ///////////////////////////////// 
        ///     GET: api/Users/5    /////
        /////////////////////////////////
        [HttpGet("{id:int}"), ActionName("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await usersRepo.GetUserById(id);
            if (user != null)
                return Ok(user);
            return BadRequest(id);
        }
        
        // Get Location

        [HttpGet("location")]
        public async Task<IActionResult> GetDriversLocation()
        {
            var users = await usersRepo.GetDriversLocations();
            if (users != null)
                return Ok(users);
            return BadRequest();
        }

        ///////////////////////////////////////////////
        ///     GET: api/users/chingiz@uber.be    /////
        ///////////////////////////////////////////////
        [HttpGet("{email}"), ActionName("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await usersRepo.GetUserByEmail(email);
            if (user != null)
                return Ok(user);
            return BadRequest(email);
        }


        ///////////////////////////////// 
        ///     POST: api/Users     /////
        /// /////////////////////////////
        [HttpPost, ActionName("CreateNewUser")]
        public async Task<IActionResult> CreateNewUserAsync([FromBody] RegistrationForm newUser)
        {
            // placeholder for new user object
            var user = new User();

            // placeholder for feedback
            var newUserSavedToDatabase = false;

            // placeholder to check the uniqueness of the new users email address
            var newUserHasUniqueEmail = true;

            // check if form was filled in correctly
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check users role and create customer or driver object accordingly
            // Create new customer object
            if (newUser.UserRoleType == 0)
            {
                //return Json("customer");
                user = new Customer
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Password = newUser.Password,
                    UserRoleType = UserRoleTypes.CUSTOMER,
                    RegistrationDate = System.DateTime.Now,
                    ContactInformation = new ContactInformation
                    {
                        PhoneNumber = newUser.PhoneNumber,
                        Email = newUser.Email,
                        Address = new Address
                        {
                            StreetName = newUser.StreetName,
                            StreetNumber = newUser.StreetNumber,
                            ZipCode = newUser.ZipCode
                        }
                    }
                };
            }

            // Create new driver object
            else if (newUser.UserRoleType == 1)
            {
                //return Json("driver");
                user = new Driver
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Password = newUser.Password,
                    UserRoleType = UserRoleTypes.DRIVER,
                    RegistrationDate = DateTime.Now,
                    IsApproved = false,
                    ContactInformation = new ContactInformation
                    {
                        PhoneNumber = newUser.PhoneNumber,
                        Email = newUser.Email,
                        Address = new Address
                        {
                            StreetName = newUser.StreetName,
                            StreetNumber = newUser.StreetNumber,
                            ZipCode = newUser.ZipCode
                        }
                    }
                };
            }

            // Get all existing users and check if new users email hasn't been used yet
            var existingUsers = await context.Users
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .ToListAsync();

            foreach (var u in existingUsers)
            {
                if (u.ContactInformation.Email == user.ContactInformation.Email)
                {
                    newUserHasUniqueEmail = false;
                }
            }

            // Add new user to the database only if his email is unique
            if (newUserHasUniqueEmail)
            {
                try
                {
                    // Check if user is Customer or Driver
                    if (user.GetType() == typeof(Customer))
                    {
                        context.Users.Add(user);
                    }
                    else if (user.GetType() == typeof(Driver))
                    {
                        context.Users.Add(user);
                    }

                    context.SaveChanges();
                    newUserSavedToDatabase = true;
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest(ex);
                }
            } else
            {
                return BadRequest("Email must be unique");
            }

            return Ok(newUserSavedToDatabase);
        }

        // POST: api/Users/sanjy-driver@uber.be
        [HttpPost("{email}")]
        public async Task<IActionResult> UserLogin(string email, [FromBody]LoginForm loginUser)
        {
            User user = await usersRepo.GetUserByEmail(email);
            if (user == null)
                return Ok("You have to register first");

            bool canAccess = usersRepo.CanUserLogin(user, loginUser);

            return Ok(canAccess);


        }

        // PUT: api/Users/sanjy-driver@uber.be
        [HttpPut("{email}")]
        public async Task<IActionResult> EditUserByEmail(string email, [FromBody]RegistrationForm newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await usersRepo.GetUserByEmail(email);

            if (user == null)
                return NotFound(false);

            // Update Customer object
            if (user.UserRoleType == UserRoleTypes.CUSTOMER)
            {
                var customer = mapper.Map<RegistrationForm, Customer>(newUser, (Customer) user);
                context.Users.Update(customer);
                await context.SaveChangesAsync();
                return Ok(true);

            }

            // Update driver object
            else if (user.UserRoleType == UserRoleTypes.DRIVER)
            {
                var driver = mapper.Map<RegistrationForm, Driver>(newUser, (Driver) user);
                context.Users.Update(driver);
                await context.SaveChangesAsync();
                return Ok(true);
            }
            return BadRequest(false);
        }
    
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            var user = await usersRepo.GetUserByEmail(email);
            if (user != null)
            {
                //context.Remove(user);
                context.RemoveRange(user.ContactInformation.Address);
                context.RemoveRange(user.ContactInformation);
                context.RemoveRange(user);
                context.SaveChanges();
                return Ok(true);
            }
            return NotFound(false);
        }
    }
}
