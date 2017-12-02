﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WebApi.Data;

namespace WebApi.Data.Migrations
{
    [DbContext(typeof(OrderContext))]
    partial class OrderContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApi.Models.Address", b =>
                {
                    b.Property<int>("AddressID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.HasKey("AddressID");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("WebApi.Models.ContactInformation", b =>
                {
                    b.Property<int>("ContactInformationID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AddressID");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(13);

                    b.HasKey("ContactInformationID");

                    b.HasIndex("AddressID");

                    b.ToTable("ContactInformation");
                });

            modelBuilder.Entity("WebApi.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ContactInformationID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("LocationID");

                    b.Property<string>("Password");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("CustomerID");

                    b.HasIndex("ContactInformationID");

                    b.HasIndex("LocationID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("WebApi.Models.Driver", b =>
                {
                    b.Property<int>("DriverID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ContactInformationID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("IsApproved");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("LocationID");

                    b.Property<string>("Password");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("DriverID");

                    b.HasIndex("ContactInformationID");

                    b.HasIndex("LocationID");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("WebApi.Models.Orders.DriverFlavour", b =>
                {
                    b.Property<int>("FlavourID");

                    b.Property<int>("DriverID");

                    b.Property<double>("Price");

                    b.HasKey("FlavourID", "DriverID");

                    b.HasIndex("DriverID");

                    b.ToTable("DriverFlavours");
                });

            modelBuilder.Entity("WebApi.Models.Orders.Flavour", b =>
                {
                    b.Property<int>("FlavourID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.HasKey("FlavourID");

                    b.ToTable("Flavours");
                });

            modelBuilder.Entity("WebApi.Models.Orders.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerrID");

                    b.Property<int>("DriverID");

                    b.Property<int?>("LocationID");

                    b.Property<double>("TotalPrice");

                    b.HasKey("OrderID");

                    b.HasIndex("CustomerrID");

                    b.HasIndex("LocationID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("WebApi.Models.Orders.OrderItem", b =>
                {
                    b.Property<int>("OrderItemID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("OrderID");

                    b.Property<double>("TotalPrice");

                    b.HasKey("OrderItemID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("WebApi.Models.Orders.OrderItemFlavour", b =>
                {
                    b.Property<int>("OrderItemID");

                    b.Property<int>("FlavourID");

                    b.Property<int>("Amount");

                    b.HasKey("OrderItemID", "FlavourID");

                    b.HasIndex("FlavourID");

                    b.ToTable("OrderItemFlavours");
                });

            modelBuilder.Entity("WebApi.Models.Users.Location", b =>
                {
                    b.Property<int>("LocationID")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("latitude");

                    b.Property<float>("longitude");

                    b.HasKey("LocationID");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("WebApi.Models.ContactInformation", b =>
                {
                    b.HasOne("WebApi.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressID");
                });

            modelBuilder.Entity("WebApi.Models.Customer", b =>
                {
                    b.HasOne("WebApi.Models.ContactInformation", "ContactInformation")
                        .WithMany()
                        .HasForeignKey("ContactInformationID");

                    b.HasOne("WebApi.Models.Users.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationID");
                });

            modelBuilder.Entity("WebApi.Models.Driver", b =>
                {
                    b.HasOne("WebApi.Models.ContactInformation", "ContactInformation")
                        .WithMany()
                        .HasForeignKey("ContactInformationID");

                    b.HasOne("WebApi.Models.Users.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationID");
                });

            modelBuilder.Entity("WebApi.Models.Orders.DriverFlavour", b =>
                {
                    b.HasOne("WebApi.Models.Driver", "Driver")
                        .WithMany("DriverFlavours")
                        .HasForeignKey("DriverID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApi.Models.Orders.Flavour", "Flavour")
                        .WithMany("DriverFlavours")
                        .HasForeignKey("FlavourID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApi.Models.Orders.Order", b =>
                {
                    b.HasOne("WebApi.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerrID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApi.Models.Driver", "Driver")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerrID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApi.Models.Users.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationID");
                });

            modelBuilder.Entity("WebApi.Models.Orders.OrderItem", b =>
                {
                    b.HasOne("WebApi.Models.Orders.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderID");
                });

            modelBuilder.Entity("WebApi.Models.Orders.OrderItemFlavour", b =>
                {
                    b.HasOne("WebApi.Models.Orders.Flavour", "Flavour")
                        .WithMany("OrderItemFlavours")
                        .HasForeignKey("FlavourID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApi.Models.Orders.OrderItem", "OrderItem")
                        .WithMany("OrderItemFlavours")
                        .HasForeignKey("OrderItemID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
