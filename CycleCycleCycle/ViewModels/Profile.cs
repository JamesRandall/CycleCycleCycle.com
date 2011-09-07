using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.ViewModels
{
    public class Profile
    {
        public IEnumerable<RouteSummary> Favourites { get; set; }

        public IEnumerable<RouteReview> Reviews { get; set; }

        public IEnumerable<RouteSummary> Uploads { get; set; }

        public IEnumerable<Ride> Rides { get; set; } 
    }
}