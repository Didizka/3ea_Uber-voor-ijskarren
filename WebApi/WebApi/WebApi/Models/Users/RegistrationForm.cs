﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Users
{
    // Handles Post and Put requests
    public class RegistrationForm
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Voornaam moet tussen 2 en 50 karakters bevatten")]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Achternaam moet tussen 2 en 50 karakters bevatten")]
        public string LastName { get; set; }

        [StringLength(100, ErrorMessage = "Het wachtwoord moet tussen {0} en {2} karakters bevatten", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email adres is verplicht")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is niet geldig")]
        public string Email { get; set; }

        [StringLength(13, MinimumLength = 9, ErrorMessage = "Telefoonnummer moet tussen 9 en 13 cijfers bevatten")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Enkel cijfers zijn toegelaten")]
        public string PhoneNumber { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Straatnaam moet tussen 5 en 50 karakters bevatten")]
        public string StreetName { get; set; }

        [StringLength(10, MinimumLength = 1, ErrorMessage = "Huisnummer moet tussen 1 en 10 karakters bevatten")]
        public string StreetNumber { get; set; }

        [StringLength(6, MinimumLength = 4, ErrorMessage = "Postcode moet tussen 4 en 6 karakters bevatten")]
        public string ZipCode { get; set; }

        [Required]
        public int UserRoleType { get; set; }

        public bool IsApproved { get; set; }
    }
    
}
