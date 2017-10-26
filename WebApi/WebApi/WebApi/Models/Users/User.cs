using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{

    public enum UserRoleTypes
    {
        CUSTOMER = 0,
        DRIVER = 1,
        ADMIN = 2
    }


    [Table("Users")]
    public class User
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "De voornaam is verplicht")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Voornaam moet tussen 2 en 50 karakters bevatten")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "De achternaam is verplicht")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Achternaam moet tussen 2 en 50 karakters bevatten")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "Het wachtwoord is verplicht")]
        [StringLength(100, ErrorMessage = "Het wachtwoord moet tussen {0} en {2} karakters bevatten", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Navigation Properties
        [Required(ErrorMessage = "De user rol is verplicht")]
        public UserRoleTypes UserRoleType { get; set; }

        public ContactInformation ContactInformation { get; set; }
    }
}
