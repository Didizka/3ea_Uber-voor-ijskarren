using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Users;

namespace WebApi.Data.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
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
            /*CreateMap<RegistrationForm, D>()
                .ForMember(ur => ur.ContactInformation, opt => opt
                    .MapFrom(rf => new ContactInformation
                    {
                        PhoneNumber = rf.PhoneNumber,
                        Email = rf.Email,
                        Address = new Address { StreetName = rf.StreetName, StreetNumber = rf.StreetNumber, ZipCode = rf.ZipCode }
                    }));*/
        }
    }
}
