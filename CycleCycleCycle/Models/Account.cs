using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public string Username { get; set; }

        public virtual ICollection<Route> Routes { get; set; }
        public virtual ICollection<RouteReview> RouteReviews { get; set; }
        public virtual ICollection<Favourite> Favourites { get; set; }
        public virtual ICollection<Ride> Rides { get; set; } 

        public override string ToString()
        {
            return Username;
        }
    }
}