using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.Models
{
    public class Favourite
    {
        public int FavouriteID { get; set; }
        public int RouteID { get; set; }
        public int AccountID { get; set; }

        public virtual Route Route { get; set; }
        public virtual Account Account { get; set; }
    }
}