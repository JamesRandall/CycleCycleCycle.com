using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.ViewModels
{
    public class RouteSummary
    {
        public int RouteID { get; set; }

        public double DistanceFromStart { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public double? AverageRating { get; set; }

        public DateTime DateCreated { get; set; }

        public bool Reviewed { get; set; }
    }
}