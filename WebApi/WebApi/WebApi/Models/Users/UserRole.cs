using System;
using System.Collections.Generic;
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

    [Table("UserRoles")]
    public class UserRole
    {
        public int UserRoleID { get; set; }

        [Required]
        public UserRoleTypes UserRoleType { get; set; }

        // Navigation Properties
        public ICollection<User> Users { get; set; }
    }
}