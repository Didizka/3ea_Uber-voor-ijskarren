using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Reviews
{
    public class DriverReview
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string DriverReviewID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int DriverID { get; set; }

        [Required]
        public string Beoordeling { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public bool Done { get; set; }

        public Driver driver { get; set; }
    }
}
