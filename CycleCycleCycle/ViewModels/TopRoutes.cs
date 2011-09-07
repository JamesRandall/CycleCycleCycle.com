using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.ViewModels
{
    public class TopRoutes
    {
        public IEnumerable<RouteSummary> HighestRated { get; set; }

        public IEnumerable<RouteSummary> RecentlyUploaded { get; set; }

    }
}