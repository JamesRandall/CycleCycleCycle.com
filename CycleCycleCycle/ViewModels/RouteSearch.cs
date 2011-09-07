using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.ViewModels
{
    public class RouteSearch
    {
        public string SearchText { get; set; }

        public string SearchedLocation { get; set; }

        public List<RouteSummary> Results { get; set; }
    }
}