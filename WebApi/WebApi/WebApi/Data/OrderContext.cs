using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Orders;

namespace WebApi.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

        public DbSet<Flavour> Flavours { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItemFlavour> OrderItemFlavours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<OrderItemFlavour>()
                .HasKey(k => new { k.OrderItemID, k.FlavourID });

            modelBuilder.Entity<OrderItemFlavour>()
                .HasOne(m => m.Flavour)
                .WithMany(mr => mr.OrderItemFlavours)
                .HasForeignKey(pt => pt.FlavourID);

            modelBuilder.Entity<OrderItemFlavour>()
                .HasOne(m => m.OrderItem)
                .WithMany(mr => mr.OrderItemFlavours)
                .HasForeignKey(pt => pt.OrderItemID);
        }
    }
}
