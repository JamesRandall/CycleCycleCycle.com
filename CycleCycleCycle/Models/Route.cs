using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.Models
{
    public class Route
    {
        public int RouteID { get; set; }
        public int? AccountID { get; set; }

        public string Name { get; set; }
        [DisplayName("Date Uploaded")]
        public DateTime DateCreated { get; set; }
        public double Distance { get; set; }
        [DisplayName("Total Ascent")]
        public double TotalAscent { get; set; }
        [DisplayName("Total Descent")]
        public double TotalDescent { get; set; }
        [DisplayName("Average Rating")]
        public double? AverageRating { get; set; }

        [DisplayName("Uploaded By")]
        public virtual Account Account { get; set; }
        public virtual ICollection<RoutePoint> RoutePoints { get; set; }
        public virtual ICollection<RouteReview> RouteReviews { get; set; }
        public virtual ICollection<Favourite> Favourites { get; set; }
        public virtual ICollection<Ride> Rides { get; set; } 

    }
}