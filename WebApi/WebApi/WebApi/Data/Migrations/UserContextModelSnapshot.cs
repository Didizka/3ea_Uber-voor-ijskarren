﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Data.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
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

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("ContactInformation");
                });

            modelBuilder.Entity("WebApi.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ContactInformationID");

                    b.Property<string>("Discriminator")
                        .IsRequired();

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

                    b.Property<int>("UserRoleType");

                    b.HasKey("UserID");

                    b.HasIndex("ContactInformationID");

                    b.HasIndex("LocationID");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
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

            modelBuilder.Entity("WebApi.Models.Customer", b =>
                {
                    b.HasBaseType("WebApi.Models.User");


                    b.ToTable("Customers");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("WebApi.Models.Driver", b =>
                {
                    b.HasBaseType("WebApi.Models.User");

                    b.Property<bool>("IsApproved");

                    b.ToTable("Drivers");

                    b.HasDiscriminator().HasValue("Driver");
                });

            modelBuilder.Entity("WebApi.Models.ContactInformation", b =>
                {
                    b.HasOne("WebApi.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressID");
                });

            modelBuilder.Entity("WebApi.Models.User", b =>
                {
                    b.HasOne("WebApi.Models.ContactInformation", "ContactInformation")
                        .WithMany()
                        .HasForeignKey("ContactInformationID");

                    b.HasOne("WebApi.Models.Users.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationID");
                });
        }
    }
}
