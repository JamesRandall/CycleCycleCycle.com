using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.Models
{
    public class RoutePoint
    {
        public int RoutePointID { get; set; }
        public int RouteID { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Elevation { get; set; }
        public DateTime TimeRecorded { get; set; }
        public int SequenceIndex { get; set; }

        public virtual Route Route { get; set; }
    }
}