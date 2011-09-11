using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using CycleCycleCycle.Models;
using CycleCycleCycle.Services.Utilities;

namespace CycleCycleCycle.Services.RouteCreators
{
    public class GpxRouteCreator : BaseRouteCreator, IRouteCreator
    {
        private const string Gpx_1_0 = "http://www.topografix.com/GPX/1/0";
        private const string Gpx_1_1 = "http://www.topografix.com/GPX/1/1";

        public GpxRouteCreator(IDistanceCalculator distanceCalculator) : base(distanceCalculator)
        {
            
        }

        public Route CreateFromXDocument(XDocument routeDocument)
        {
            XmlNamespaceManager namespaces = new XmlNamespaceManager(new NameTable());
            XNamespace ns = routeDocument.Root.GetDefaultNamespace();
            Route route = null;
            if (ns.NamespaceName.Equals(Gpx_1_0))
            {
                route = CreateGpx_1_0_Route(routeDocument, ns);
            }
            else if (ns.NamespaceName.Equals(Gpx_1_1))
            {
                route = CreateGpx_1_1_Route(routeDocument, ns);
            }
            else
            {
                throw new RouteCreatorException(String.Format("Gpx namespace not recognized: {0}", ns.NamespaceName));
            }
            return route;
        }

        private Route CreateGpx_1_0_Route(XDocument routeDocument, XNamespace ns)
        {
            XElement track = routeDocument.Root.Element(ns + "trk");
            string name = track.Element(ns + "name").Value;

            DateTime time = DateTime.Parse(routeDocument.Root.Element(ns + "time").Value);

            Route newRoute = new Route { Name = name, DateCreated = DateTime.Now };
            List<RoutePoint> routePoints = new List<RoutePoint>();
            var trackPoints = track.Element(ns + "trkseg").Elements(ns + "trkpt");
            foreach (XElement trackPoint in trackPoints)
            {
                RoutePoint routePoint = new RoutePoint();
                routePoint.Latitude = double.Parse(trackPoint.Attribute("lat").Value);
                routePoint.Longitude = double.Parse(trackPoint.Attribute("lon").Value);
                routePoint.Elevation = double.Parse(trackPoint.Element(ns + "ele").Value);
                routePoint.TimeRecorded = time;
                routePoints.Add(routePoint);

                time = time.AddSeconds(1); // purely arbitary really
            }
            routePoints = routePoints.OrderBy(rp => rp.TimeRecorded).ToList();
            int index = 0;
            foreach (RoutePoint routePoint in routePoints)
            {
                routePoint.SequenceIndex = index;
                index++;
            }
            newRoute.RoutePoints = routePoints;
            newRoute.Distance = CalculateDistance(newRoute.RoutePoints);
            Tuple<double, double> ascentDescent = CalculateAscentDescent(newRoute.RoutePoints);
            newRoute.TotalAscent += ascentDescent.Item1;
            newRoute.TotalDescent += ascentDescent.Item2;
            return newRoute;
        }

        private Route CreateGpx_1_1_Route(XDocument routeDocument, XNamespace ns)
        {
            XElement track = routeDocument.Root.Element(ns + "trk");
            string name = track.Element(ns + "name").Value;

            Route newRoute = new Route {Name = name, DateCreated = DateTime.Now};
            List<RoutePoint> routePoints = new List<RoutePoint>();
            var trackPoints = track.Element(ns + "trkseg").Elements(ns + "trkpt");
            foreach (XElement trackPoint in trackPoints)
            {
                RoutePoint routePoint = new RoutePoint();
                routePoint.Latitude = double.Parse(trackPoint.Attribute("lat").Value);
                routePoint.Longitude = double.Parse(trackPoint.Attribute("lon").Value);
                routePoint.Elevation = double.Parse(trackPoint.Element(ns + "ele").Value);
                routePoint.TimeRecorded = DateTime.Parse(trackPoint.Element(ns + "time").Value);
                routePoints.Add(routePoint);
            }
            routePoints = routePoints.OrderBy(rp => rp.TimeRecorded).ToList();
            int index = 0;
            foreach (RoutePoint routePoint in routePoints)
            {
                routePoint.SequenceIndex = index;
                index++;
            }
            newRoute.RoutePoints = routePoints;
            newRoute.Distance = CalculateDistance(newRoute.RoutePoints);
            Tuple<double, double> ascentDescent = CalculateAscentDescent(newRoute.RoutePoints);
            newRoute.TotalAscent += ascentDescent.Item1;
            newRoute.TotalDescent += ascentDescent.Item2;
            return newRoute;
        }
    }
}