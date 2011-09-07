using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.Models
{
    public class Ride
    {
        public int RideID { get; set; }
        public int AccountID { get; set; }
        public int RouteID { get; set; }

        public DateTime TimeOfRide { get; set; }
        public TimeSpan? TimeTaken { get; set; }

        public virtual Account Account { get; set; }
        public virtual Route Route { get; set; }
    }
}