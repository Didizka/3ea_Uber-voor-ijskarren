﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Review
{
    [Table("CustomerReviews")]
    public class CustomerReview
    {
        public string CustomerReviewID { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }

        public int DriverID { get; set; }
        public Driver Driver { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
    }
}
