using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Orders.Repo;
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

            CreateMap<Driver, DriverFlavourResource>()
                .ForMember(df => df.Email, opt => opt.MapFrom(d => d.ContactInformation.Email))
                .ForMember(df => df.Flavours, opt => opt.MapFrom(d => d.DriverFlavours
                    .Select(dfr => new FlavourResource { Name = dfr.Flavour.Name, Price = dfr.Driver.DriverFlavours.Single( s=> s.FlavourID == dfr.FlavourID).Price })));
            
                //.ForMember(df => df.Flavours, opt => opt.MapFrom(d => d.DriverFlavours.Where(f => f.DriverID == d.DriverID)
        }
    }
}
