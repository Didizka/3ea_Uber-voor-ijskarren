using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Orders;

namespace WebApi.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<ContactInformation> ContactInformation { get; set; }        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<User> Users { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(a => a.ContactInformation);

            modelBuilder.Entity<User>()
                .HasOne(a => a.Location);

            modelBuilder.Entity<ContactInformation>()
                .HasOne(a => a.Address);

            modelBuilder
                .Entity<ContactInformation>()
                 .HasIndex(e => e.Email)
                 .IsUnique(true);
        }
    }

    
}
