using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.Services.RouteFileCreator
{
    public interface IRouteFileCreator
    {
        XDocument CreateRoute(Route route, out string filename);
    }
}
