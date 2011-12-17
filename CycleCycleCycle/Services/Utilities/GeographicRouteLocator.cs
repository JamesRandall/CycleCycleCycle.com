using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.ViewModels;

namespace CycleCycleCycle.Services.Utilities
{
    public class GeographicRouteLocator : IGeographicRouteLocator
    {
        private readonly IGeocodeService _geocodeService;
        private readonly IRepository<Route> _routeRepository;

        public GeographicRouteLocator(IGeocodeService geocodeService, IUnitOfWork unitOfWork)
        {
            if (geocodeService == null) throw new ArgumentNullException("geocodeService");
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            _geocodeService = geocodeService;
            _routeRepository = unitOfWork.GetRepository<Route>();
        }

        public List<RouteSummary> FindRoute(string searchString, double distanceFromStart, out string searchedLocation)
        {
            Location location = _geocodeService.Geocode(searchString);
            searchedLocation = location.Name;
            const double R = 6367;
            const double RAD = Math.PI / 180;
            Func<double, double, double, double, double> dist =
                (lat1, lon1, lat2, lon2) =>
                R * 2 *
                (
                    Math.Asin(Math.Min(1, Math.Sqrt(
                        (
                            Math.Pow(Math.Sin(((lat1 * RAD - lat2 * RAD)) / 2.0), 2.0) +
                            Math.Cos(lat1 * RAD) * Math.Cos(lat2 * RAD) *
                            Math.Pow(Math.Sin(((lon1 * RAD - lon2 * RAD)) / 2.0), 2.0)
                        )
                                              )))
                );

            List<RouteSummary> routes = _routeRepository.All.Where(
                r => R * 2 *
                     (
                         SqlFunctions.Asin(SqlFunctions.SquareRoot(
                             (
                                 Math.Pow(
                                     SqlFunctions.Sin(((location.Latitude * RAD -
                                                        r.RoutePoints.FirstOrDefault().Latitude * RAD)) / 2.0).Value,
                                     2.0) +
                                 SqlFunctions.Cos(location.Latitude * RAD).Value *
                                 SqlFunctions.Cos(r.RoutePoints.FirstOrDefault().Latitude * RAD).Value *
                                 Math.Pow(
                                     SqlFunctions.Sin(((location.Longitude * RAD -
                                                        r.RoutePoints.FirstOrDefault().Longitude * RAD)) / 2.0).Value,
                                     2.0)
                             )
                                               ))
                     ) <= distanceFromStart)
                .Select(r => new RouteSummary
                {
                    CreatedBy = r.Account == null ? "" : r.Account.Username,
                    Name = r.Name,
                    RouteID = r.RouteID,
                    DistanceFromStart = (R * 2 *
                                         (
                                             SqlFunctions.Asin(SqlFunctions.SquareRoot(
                                                 (
                                                     Math.Pow(
                                                         SqlFunctions.Sin(((location.Latitude * RAD -
                                                                            r.RoutePoints.
                                                                                FirstOrDefault().
                                                                                Latitude * RAD)) / 2.0)
                                                             .Value, 2.0) +
                                                     SqlFunctions.Cos(location.Latitude * RAD).Value *
                                                     SqlFunctions.Cos(
                                                         r.RoutePoints.FirstOrDefault().Latitude *
                                                         RAD).Value *
                                                     Math.Pow(
                                                         SqlFunctions.Sin(((location.Longitude * RAD -
                                                                            r.RoutePoints.
                                                                                FirstOrDefault().
                                                                                Longitude * RAD)) / 2.0)
                                                             .Value, 2.0)
                                                 )
                                                                   ))
                                         )).Value * 1000
                }).ToList();

            return routes;
        }
    }
}