using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.Services.RouteFileCreator
{
    public class GpxRouteFileCreator : IRouteFileCreator
    {
        public XDocument CreateRoute(Route route, out string filename)
        {
            double maxLat = route.RoutePoints.Max(rp => rp.Latitude);
            double maxLon = route.RoutePoints.Max(rp => rp.Longitude);
            double minLat = route.RoutePoints.Min(rp => rp.Latitude);
            double minLon = route.RoutePoints.Min(rp => rp.Longitude);

            XNamespace ns = XNamespace.Get("http://www.topografix.com/GPX/1/1");
            filename = route.Name + ".gpx";
            XDocument document = new XDocument(new XElement(ns + "gpx",
                    new XElement(ns + "metadata",
                        new XElement(ns + "link",
                            new XAttribute("href", "http://wwww.cyclecyclecycle.com"),
                            new XElement(ns + "text", "CycleCycleCycle.com")
                        ),
                        new XElement(ns + "bounds",
                            new XAttribute("maxlat", maxLat),
                            new XAttribute("maxlon", maxLon),
                            new XAttribute("minlat", minLat),
                            new XAttribute("minlon", minLon)
                        )
                    ),
                    new XElement(ns + "trk",
                        new XElement(ns + "name", route.Name),
                        new XElement(ns + "trkseg",
                            route.RoutePoints.OrderBy(rp => rp.SequenceIndex).Select(rp => new XElement(ns + "trkpt",
                                new XAttribute("lat", rp.Latitude),
                                new XAttribute("lon", rp.Longitude),
                                new XElement(ns + "ele", rp.Elevation),
                                new XElement(ns + "time", rp.TimeRecorded)
                            ))
                        )
                    )
                ));
            return document;
        }
    }
}