using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApi.Data.Migrations
{
    public partial class initorders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StreetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StreetNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressID);
                });

            migrationBuilder.CreateTable(
                name: "Flavours",
                columns: table => new
                {
                    FlavourID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flavours", x => x.FlavourID);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    latitude = table.Column<float>(type: "real", nullable: false),
                    longitude = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationID);
                });

            migrationBuilder.CreateTable(
                name: "ContactInformation",
                columns: table => new
                {
                    ContactInformationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressID = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInformation", x => x.ContactInformationID);
                    table.ForeignKey(
                        name: "FK_ContactInformation_Addresses_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Addresses",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContactInformationID = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_Customers_ContactInformation_ContactInformationID",
                        column: x => x.ContactInformationID,
                        principalTable: "ContactInformation",
                        principalColumn: "ContactInformationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContactInformationID = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverID);
                    table.ForeignKey(
                        name: "FK_Drivers_ContactInformation_ContactInformationID",
                        column: x => x.ContactInformationID,
                        principalTable: "ContactInformation",
                        principalColumn: "ContactInformationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drivers_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DriverFlavours",
                columns: table => new
                {
                    FlavourID = table.Column<int>(type: "int", nullable: false),
                    DriverID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverFlavours", x => new { x.FlavourID, x.DriverID });
                    table.ForeignKey(
                        name: "FK_DriverFlavours_Drivers_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Drivers",
                        principalColumn: "DriverID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverFlavours_Flavours_FlavourID",
                        column: x => x.FlavourID,
                        principalTable: "Flavours",
                        principalColumn: "FlavourID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    DriverID = table.Column<int>(type: "int", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Drivers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Drivers",
                        principalColumn: "DriverID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemID);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemFlavours",
                columns: table => new
                {
                    OrderItemID = table.Column<int>(type: "int", nullable: false),
                    FlavourID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemFlavours", x => new { x.OrderItemID, x.FlavourID });
                    table.ForeignKey(
                        name: "FK_OrderItemFlavours_Flavours_FlavourID",
                        column: x => x.FlavourID,
                        principalTable: "Flavours",
                        principalColumn: "FlavourID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemFlavours_OrderItems_OrderItemID",
                        column: x => x.OrderItemID,
                        principalTable: "OrderItems",
                        principalColumn: "OrderItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformation_AddressID",
                table: "ContactInformation",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ContactInformationID",
                table: "Customers",
                column: "ContactInformationID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LocationID",
                table: "Customers",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_DriverFlavours_DriverID",
                table: "DriverFlavours",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ContactInformationID",
                table: "Drivers",
                column: "ContactInformationID");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_LocationID",
                table: "Drivers",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemFlavours_FlavourID",
                table: "OrderItemFlavours",
                column: "FlavourID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderID",
                table: "OrderItems",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_LocationID",
                table: "Orders",
                column: "LocationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverFlavours");

            migrationBuilder.DropTable(
                name: "OrderItemFlavours");

            migrationBuilder.DropTable(
                name: "Flavours");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "ContactInformation");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
