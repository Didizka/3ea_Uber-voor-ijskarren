using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Review
{
    public class CustomerReview
    {
        public string CustomerReviewID { get; set; }

        [Required(ErrorMessage = "Review is verplicht")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Review moet tussen 2 en 50 karakters bevatten")]
        public string Review { get; set; }

        [Required(ErrorMessage = "Score is verplicht")]
        [Range(0, 5, ErrorMessage = "Can only be between 0 .. 5")]
        public int Score { get; set; }

        public int DriverID { get; set; }
        public Driver Driver { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
    }
}
