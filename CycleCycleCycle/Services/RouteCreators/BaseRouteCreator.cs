using System;
using System.Collections.Generic;
using System.Linq;
using CycleCycleCycle.Models;
using CycleCycleCycle.Services.Implementation;
using CycleCycleCycle.Services.Utilities;

namespace CycleCycleCycle.Services.RouteCreators
{
    public class BaseRouteCreator
    {
        private readonly IDistanceCalculator _distanceCalculator;

        public BaseRouteCreator(IDistanceCalculator distanceCalculator)
        {
            _distanceCalculator = distanceCalculator;
        }

        protected double CalculateDistance(IEnumerable<RoutePoint> routePoints)
        {
            if (routePoints == null || !routePoints.Any())
            {
                throw new ServiceException("Unable to calculate distance");
            }

            try
            {
                RoutePoint point1 = null;
                RoutePoint point2 = null;
                double distance = 0;

                foreach (RoutePoint rp in routePoints)
                {
                    point1 = point2;
                    point2 = rp;

                    if (point1 != null && point2 != null)
                    {
                        distance += _distanceCalculator.Calculate(point1.Latitude,
                                                                  point1.Longitude,
                                                                  point2.Latitude,
                                                                  point2.Longitude);
                    }
                }

                return distance * 1000; // distance will be in km we want it in meters
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error occurred calculating aggregate distance", ex);
            }
        }

        protected Tuple<double, double> CalculateAscentDescent(IEnumerable<RoutePoint> routePoints)
        {
            if (routePoints == null || !routePoints.Any())
            {
                throw new ServiceException("Unable to calculate distance");
            }

            RoutePoint point1 = null;
            RoutePoint point2 = null;
            double totalAscent = 0;
            double totalDescent = 0;

            foreach (RoutePoint routePoint in routePoints)
            {
                point1 = point2;
                point2 = routePoint;

                if (point1 != null && point2 != null)
                {
                    if (point2.Elevation > point1.Elevation)
                    {
                        totalAscent += point2.Elevation - point1.Elevation;
                    }
                    else if (point2.Elevation < point1.Elevation)
                    {
                        totalDescent += point1.Elevation - point2.Elevation;
                    }
                }
            }

            return new Tuple<double, double>(totalAscent, totalDescent);
        }
    }
}