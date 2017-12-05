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
using WebApi.Models.Orders;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly UserContext context;
        private readonly IMapper mapper;
        private readonly IUsersRepository usersRepo;
        private readonly IOrderRepository orderRepository;

        public UsersController(UserContext _context, IMapper _mapper, IUsersRepository usersRepo, IOrderRepository orderRepository) {
            context = _context;
            mapper = _mapper;
            this.usersRepo = usersRepo;
            this.orderRepository = orderRepository;
        }

        //////////////////////////////////// 
        ///     GET: api/Users      ////////
        //////////////////////////////////// 
        [HttpGet("customers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await usersRepo.GetCustomers();
                    
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpGet("drivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var result = await usersRepo.GetDrivers();

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
            var customer = await usersRepo.GetCustomerById(id);
            usersRepo.RemovePasswordOfCustomer(customer);
            if (customer != null)
                return Ok(customer);
            var driver = await usersRepo.GetDriverById(id);
            usersRepo.RemovePasswordOfDriver(driver);
            if (driver != null)
                return Ok(driver);
            return NotFound(id);
        }

        ///////////////////////////////////////////////
        ///     GET: api/users/location           /////
        ///////////////////////////////////////////////
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
            var customerOrDriver = await usersRepo.CustomerOrDriver(email);
            if (customerOrDriver == UserRoleTypes.NOTFOUND)
                return NotFound(email);
            if(customerOrDriver == UserRoleTypes.CUSTOMER)
                return Ok(await usersRepo.GetCustomerByEmail(email));
            else if(customerOrDriver == UserRoleTypes.DRIVER)
                return Ok(await usersRepo.GetDriverByEmail(email));

            return BadRequest();
            
        }


        ///////////////////////////////// 
        ///     POST: api/Users     /////
        /// /////////////////////////////
        [HttpPost, ActionName("CreateNewUser")]
        public async Task<IActionResult> CreateNewUserAsync([FromBody] RegistrationForm newUser)
        {

            // placeholder for feedback
            var newUserSavedToDatabase = false;

            // check if form was filled in correctly
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customerOrDriver = await usersRepo.CustomerOrDriver(newUser.Email);
            if (customerOrDriver == UserRoleTypes.CUSTOMER || customerOrDriver == UserRoleTypes.DRIVER)
                return BadRequest("Email must be unique");

            // Check users role and create customer or driver object accordingly
            // Create new customer object
            if (newUser.UserRoleType == (int)UserRoleTypes.CUSTOMER)
            {
                Customer user = new Customer
                {
                    RegistrationDate = DateTime.Now,
                    Location = new Location { latitude = (float)(51.2001783), longitude = (float)4.4327702 },
                    ContactInformation = new ContactInformation { Address = new Address() }
                };
                var customer = mapper.Map<RegistrationForm, Customer>(newUser, user);

                try
                {
                    await context.Customers.AddAsync(customer);
                    await context.SaveChangesAsync();
                    newUserSavedToDatabase = true;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }


            }

            // Create new driver object
            else if (newUser.UserRoleType == (int)UserRoleTypes.DRIVER)
            {
                Driver user = new Driver
                {
                    RegistrationDate = DateTime.Now,
                    Location = new Location { latitude = (float)(51.2301299), longitude = (float)(4.4161723) },
                    ContactInformation = new ContactInformation { Address = new Address() }
                };
                var driver = mapper.Map<RegistrationForm, Driver>(newUser, user);
                var flavours = await orderRepository.GetFlavoursAsync();
                

                try
                {
                    await context.Drivers.AddAsync(driver);
                    await context.SaveChangesAsync();
                    newUserSavedToDatabase = true;

                    foreach (var flavour in flavours)
                    {
                        await context.DriverFlavours.AddAsync(new DriverFlavour
                        {
                            DriverID = driver.DriverID,
                            FlavourID = flavours.Single(f => f.Name == flavour.Name).FlavourID,
                            Price = flavour.Price
                        });
                    }
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }

            return Ok(newUserSavedToDatabase);
        }

        // POST: api/Users/sanjy-driver@uber.be
        [HttpPost("{email}")]
        public async Task<IActionResult> UserLogin(string email, [FromBody]LoginForm loginUser)
        {
            var customerOrDriver = await usersRepo.CustomerOrDriver(loginUser.Email);
            if (customerOrDriver == UserRoleTypes.NOTFOUND)
                return Ok("You have to register first");
            bool canAccess = false;
            if (customerOrDriver != UserRoleTypes.NOTFOUND)
            {
                canAccess = await usersRepo.CanUserLogin(loginUser, customerOrDriver);
                return Ok(customerOrDriver.ToString());

            }
            return Ok(canAccess);
        }

        // PUT: api/Users/sanjy-driver@uber.be
        [HttpPut("{email}")]
        public async Task<IActionResult> EditUserByEmail(string email, [FromBody]RegistrationForm newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (newUser.UserRoleType == (int)UserRoleTypes.CUSTOMER)
            {
                var user = await usersRepo.GetCustomerByEmail(email);
                if (user == null)
                    return NotFound(false);
                var customer = mapper.Map<RegistrationForm, Customer>(newUser, user);
                context.Customers.Update(customer);
                await context.SaveChangesAsync();
                return Ok(true);

            }

            // Update driver object
            else if (newUser.UserRoleType == (int)UserRoleTypes.DRIVER)
            {
                var user = await usersRepo.GetDriverByEmail(email);
                if (user == null)
                    return NotFound(false);
                var driver = mapper.Map<RegistrationForm, Driver>(newUser, (Driver)user);
                context.Drivers.Update(driver);
                await context.SaveChangesAsync();
                return Ok(true);
            }
            return BadRequest(false);
        }
    
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            var customer = await usersRepo.GetCustomerByEmail(email);
            if (customer != null)
            {
                //context.Remove(user);
                context.RemoveRange(customer.ContactInformation.Address);
                context.RemoveRange(customer.ContactInformation);
                context.RemoveRange(customer);
                context.SaveChanges();
                return Ok(true);
            }
            var driver = await usersRepo.GetDriverByEmail(email);
            if (driver != null)
            {
                //context.Remove(user);
                context.RemoveRange(driver.ContactInformation.Address);
                context.RemoveRange(driver.ContactInformation);
                context.RemoveRange(driver);
                context.SaveChanges();
                return Ok(true);
            }
            return NotFound(false);
        }
    }
}
