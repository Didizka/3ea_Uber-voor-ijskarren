using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("Users")]
    public class User
    {
        public int UserID { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Voornaam moet tussen 2 en 50 karakters bevatten")]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Achternaam moet tussen 2 en 50 karakters bevatten")]
        public string LastName { get; set; }

        public DateTime RegistrationDate { get; set; }

        [StringLength(100, ErrorMessage = "Het wachtwoord moet tussen {0} en {2} karakters bevatten", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Navigation Property
        public UserRole UserRole { get; set; }

        public ContactInformation ContactInformation { get; set; }
    }
}
