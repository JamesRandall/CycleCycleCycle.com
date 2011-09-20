using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.ViewModels
{
    public class Profile
    {
        public double TotalDistanceCycled { get; set; }

        public double TotalAscent { get; set; }

        public double TotalDescent { get; set; }

        public IEnumerable<RouteSummary> Favourites { get; set; }

        public IEnumerable<RouteReview> Reviews { get; set; }

        public IEnumerable<RouteSummary> Uploads { get; set; }

        public IEnumerable<Ride> Rides { get; set; }

        public bool IsOwnProfile { get; set; }

        public string Username { get; set; }
    }
}