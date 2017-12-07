﻿using AutoMapper;
using System.Linq;
using WebApi.Models;
using WebApi.Models.Orders;
using WebApi.Models.Orders.Resources;
using WebApi.Models.Users;

namespace WebApi.Data.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            /*CreateMap<RegistrationForm, Driver>()
                .ForMember(d => d.IsApproved, opt => opt.MapFrom( rf => rf.IsApproved))
                .ForMember(d => d.ContactInformation, opt => opt.MapFrom( 
                    rf => new ContactInformation
                    {
                        Email = rf.Email,
                        PhoneNumber = rf.PhoneNumber,
                        Address = new Address { StreetName = rf.StreetName, StreetNumber = rf.StreetNumber, ZipCode = rf.ZipCode }
                    })) ;*/

            CreateMap<RegistrationForm, Driver>()
                .AfterMap((dest, u) =>
                {
                    u.ContactInformation.Email = dest.Email;
                    u.ContactInformation.PhoneNumber = dest.PhoneNumber;
                    u.ContactInformation.Address.StreetName = dest.StreetName;
                    u.ContactInformation.Address.StreetNumber = dest.StreetNumber;
                    u.ContactInformation.Address.ZipCode = dest.ZipCode;
                    u.IsApproved = dest.IsApproved;
                });
            CreateMap<RegistrationForm, Customer>()
                .AfterMap((dest, u) =>
                {
                    u.ContactInformation.Email = dest.Email;
                    u.ContactInformation.PhoneNumber = dest.PhoneNumber;
                    u.ContactInformation.Address.StreetName = dest.StreetName;
                    u.ContactInformation.Address.StreetNumber = dest.StreetNumber;
                    u.ContactInformation.Address.ZipCode = dest.ZipCode;
                });
            CreateMap<DriverResource, OrderTotalPriceResource>();

            CreateMap<Customer, CustomerResource>()
                .ForMember(d => d.Email, opt => opt.MapFrom( d => d.ContactInformation.Email));

            CreateMap<Driver, DriverResource>()
                .ForMember(df => df.Email, opt => opt.MapFrom(d => d.ContactInformation.Email))
                .ForMember(df => df.Flavours, opt => opt.MapFrom(d => d.DriverFlavours
                    .Select(dfr => new FlavourResource { Name = dfr.Flavour.Name, Price = dfr.Driver.DriverFlavours.Single( s=> s.FlavourID == dfr.FlavourID).Price })));


            CreateMap<Order, ShoppingCart>()
                .ForMember(d => d.Cart, opt => opt.MapFrom(
                    rf => rf.OrderItems.Select(oi => new Icecream
                    {
                        IceCream = oi.OrderItemFlavours.Select(oif => new FlavourFrountend
                        {
                            Name = oif.Flavour.Name,
                            Amount = oif.Amount,
                            //Price = oif.Amount * 1.2
                            //Price = rf.Driver.DriverFlavours.Single(s => s.Flavour.Name == "Test").Price
                        }).ToArray()
                    })));

            CreateMap<Order, OrderResource>()
                 .AfterMap((o, or) =>
                 {
                     or.Driver = Mapper.Map<Driver, DriverResource>(o.Driver);
                     or.ShoppingCart = Mapper.Map<Order, ShoppingCart>(o);
                     foreach (var icecream in or.ShoppingCart.Cart)
                     {
                         foreach (var flavour in icecream.IceCream)
                         {
                             flavour.Price = o.Driver.DriverFlavours.Single(s => s.Flavour.Name == flavour.Name).Price * flavour.Amount;
                         }
                     }
                 });
                
    
        }
    }
}
