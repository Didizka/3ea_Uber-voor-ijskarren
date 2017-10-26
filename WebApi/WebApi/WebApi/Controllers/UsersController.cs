using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly UserContext context;

        public UsersController(UserContext _context) {
            context = _context;
        }

        //////////////////////////////////// 
        ///     GET: api/Users      ////////
        //////////////////////////////////// 
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await context.Users
                            .Include(c => c.ContactInformation)
                                   .ThenInclude(a => a.Address)
                            .ToListAsync();
                    
            return Json(result);
        }

        ///////////////////////////////// 
        ///     GET: api/Users/5    /////
        /// /////////////////////////////
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }


        ///////////////////////////////// 
        ///     POST: api/Users     /////
        /// /////////////////////////////
        [HttpPost]
        public IActionResult Post([FromBody] RegistrationForm newUser)
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
                } else if (user.GetType() == typeof(Driver))
                {
                    context.Users.Add(user);
                }

                context.SaveChanges();
                newUserSavedToDatabase = true;
            } catch (DbUpdateConcurrencyException ex)
            {
                // Log exception
            }

            return Json(newUserSavedToDatabase);
        }
        
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
