using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.Models
{
    public class RouteReview
    {
        public int RouteReviewID { get; set; }
        public int RouteID { get; set; }
        public int AccountID { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }
        
        [StringLength(1024)]
        [DisplayName("Comments")]
        public string Review { get; set; }

        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }

        public virtual Route Route { get; set; }
        public virtual Account Account { get; set; }
    }
}