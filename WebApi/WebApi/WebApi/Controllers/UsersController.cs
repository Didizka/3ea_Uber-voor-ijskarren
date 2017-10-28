using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Users;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly UserContext context;
        private readonly IMapper mapper;

        public UsersController(UserContext _context, IMapper _mapper) {
            context = _context;
            mapper = _mapper;
        }

        //////////////////////////////////// 
        ///     GET: api/Users      ////////
        //////////////////////////////////// 
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await context.Users
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .ToListAsync();
                    
            if (result == null)
            {
                return BadRequest();
            }

            return NotFound(false);
        }

        ///////////////////////////////// 
        ///     GET: api/Users/5    /////
        /////////////////////////////////
        [HttpGet("{id:int}"), ActionName("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await context.Users.Where(u => u.UserID == id)
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .ToListAsync();
            if (user != null)
                return Ok(user);
            return BadRequest(id);
        }

        ///////////////////////////////////////////////
        ///     GET: api/users/chingiz@uber.be    /////
        ///////////////////////////////////////////////
        [HttpGet("{email}"), ActionName("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await context.Users.Where(u => u.ContactInformation.Email == email)
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .ToListAsync();
            if (user != null)
                return Ok(user);
            return BadRequest(email);
        }


        ///////////////////////////////// 
        ///     POST: api/Users     /////
        /// /////////////////////////////
        [HttpPost, ActionName("CreateNewUser")]
        public IActionResult CreateNewUser([FromBody] RegistrationForm newUser)
        {
            // placeholder for new user object
            var user = new User();

            // placeholder for feedback
            var newUserSavedToDatabase = false;

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
                    RegistrationDate = System.DateTime.Now,
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
                // write custom message? OR check before saving
                // Normally here only log info
                return BadRequest(ex.InnerException.Message);
            }

            return Ok(newUserSavedToDatabase);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUserById(int id, [FromBody]RegistrationForm newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await context.Users
                .Include(v => v.ContactInformation)
                    .ThenInclude(vf => vf.Address)
                .SingleOrDefaultAsync(v => v.UserID == id);

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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var user = await context.Users
                .Include(v => v.ContactInformation)
                    .ThenInclude(vf => vf.Address)
                .SingleOrDefaultAsync(v => v.UserID == id);
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
