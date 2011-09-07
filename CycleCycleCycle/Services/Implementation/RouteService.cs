using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Security;
using System.Xml.Linq;
using CycleCycleCycle.Cacheing;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services.RouteCreators;
using CycleCycleCycle.Services.Utilities;
using CycleCycleCycle.ViewModels;
using CycleCycleCycle.ViewModels.Mappers;

namespace CycleCycleCycle.Services.Implementation
{
    public class RouteService : IRouteService
    {
        private readonly IRouteCreator _routeCreator;
        private readonly IHeightMapImageBuilder _heightMapImageBuilder;
        private readonly IHeightMapImageCache _heightMapImageCache;
        private readonly IGeocodeService _geocodeService;
        private readonly IRouteToRouteResultMapper _routeToRouteResultMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISortSecurity _sortSecurity;
        private readonly IRepository<Route> _routeRepository;
        private readonly IRepository<Favourite> _favouriteRepository;
        private readonly IRepository<RoutePoint> _routePointRepository; 

        public RouteService(IRouteCreator routeCreator,
            IHeightMapImageBuilder heightMapImageBuilder,
            IHeightMapImageCache heightMapImageCache,
            IGeocodeService geocodeService,
            IRouteToRouteResultMapper routeToRouteResultMapper,
            IUnitOfWork unitOfWork,
            ISortSecurity sortSecurity)
        {
            _routeRepository = unitOfWork.GetRepository<Route>();
            _favouriteRepository = unitOfWork.GetRepository<Favourite>();
            _routePointRepository = unitOfWork.GetRepository<RoutePoint>();
            _routeCreator = routeCreator;
            _heightMapImageBuilder = heightMapImageBuilder;
            _heightMapImageCache = heightMapImageCache;
            _geocodeService = geocodeService;
            _routeToRouteResultMapper = routeToRouteResultMapper;
            _unitOfWork = unitOfWork;
            _sortSecurity = sortSecurity;
        }

        public Route CreateFromXDocument(XDocument routeDocument, int accountId)
        {
            try
            {
                Route route = _routeCreator.CreateFromXDocument(routeDocument);
                route.AccountID = accountId;
                _routeRepository.InsertOrUpdate(route, (r) => r.RouteID);
                _unitOfWork.Save();
                return route;
            }
            catch (Exception ex)
            {   
                throw new ServiceException("Unable to built a route from the XML document", ex);
            }
        }

        public Bitmap HeightMapImage(int routeId, int width, int height)
        {
            if (width > 1024 || height > 1024)
                throw new ServiceException("Requested height map is too large. Maximum size is 1024x1024 pixels");

            try
            {
                Route route = _routeRepository.Find(routeId);
                Bitmap rc = _heightMapImageCache.HeightMapImage(route, width, height);
                if (rc == null)
                {
                    rc = _heightMapImageBuilder.HeightMapImage(route, width, height);
                    _heightMapImageCache.CacheHeightMapImage(rc, route, width, height);
                }
                return rc;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Unable to build height profile bitmap.", ex);
            }
        }

        public TopRoutes TopRoutes(Account account)
        {
            const int numberToTake = 20;
            TopRoutes topRoutes = new TopRoutes
            {
                RecentlyUploaded =
                    _routeRepository.AllIncluding(route => route.Account).OrderByDescending(
                        route => route.DateCreated).Take(numberToTake).ToList().Select(r => _routeToRouteResultMapper.FromRoute(r, account)),
                HighestRated =
                    _routeRepository.AllIncluding(route => route.Account).OrderByDescending(
                        route => route.AverageRating).ThenByDescending(
                            route => route.DateCreated).Take(numberToTake).ToList().Select(r => _routeToRouteResultMapper.FromRoute(r, account))
            };

            return topRoutes;
        }

        public List<RouteSummary> FindRoute(string searchString, double distanceFromStart, out string searchedLocation)
        {
            try
            {
                Location location = _geocodeService.Geocode(searchString);
                searchedLocation = location.Name;
                const double R = 6367; 
                const double RAD = Math.PI / 180;
                Func<double, double, double, double, double> dist =
                    (lat1, lon1, lat2, lon2) =>
                    R*2*
                    (
                        Math.Asin(Math.Min(1, Math.Sqrt(
                            (
                                Math.Pow(Math.Sin(((lat1*RAD - lat2*RAD))/2.0), 2.0) +
                                Math.Cos(lat1*RAD)*Math.Cos(lat2*RAD)*
                                Math.Pow(Math.Sin(((lon1*RAD - lon2*RAD))/2.0), 2.0)
                            )
                                                  )))
                    );

                List<RouteSummary> routes = _routeRepository.All.Where(
                    r => R*2*
                         (
                             SqlFunctions.Asin(SqlFunctions.SquareRoot(
                                 (
                                     Math.Pow(
                                         SqlFunctions.Sin(((location.Latitude*RAD -
                                                            r.RoutePoints.FirstOrDefault().Latitude*RAD))/2.0).Value,
                                         2.0) +
                                     SqlFunctions.Cos(location.Latitude*RAD).Value*
                                     SqlFunctions.Cos(r.RoutePoints.FirstOrDefault().Latitude*RAD).Value*
                                     Math.Pow(
                                         SqlFunctions.Sin(((location.Longitude*RAD -
                                                            r.RoutePoints.FirstOrDefault().Longitude*RAD))/2.0).Value,
                                         2.0)
                                 )
                                                   ))
                         ) <= distanceFromStart)
                    .Select(r => new RouteSummary
                                     {
                                         CreatedBy = r.Account == null ? "" : r.Account.Username,
                                         Name = r.Name,
                                         RouteID = r.RouteID,
                                         DistanceFromStart = (R*2*
                                                              (
                                                                  SqlFunctions.Asin(SqlFunctions.SquareRoot(
                                                                      (
                                                                          Math.Pow(
                                                                              SqlFunctions.Sin(((location.Latitude*RAD -
                                                                                                 r.RoutePoints.
                                                                                                     FirstOrDefault().
                                                                                                     Latitude*RAD))/2.0)
                                                                                  .Value, 2.0) +
                                                                          SqlFunctions.Cos(location.Latitude*RAD).Value*
                                                                          SqlFunctions.Cos(
                                                                              r.RoutePoints.FirstOrDefault().Latitude*
                                                                              RAD).Value*
                                                                          Math.Pow(
                                                                              SqlFunctions.Sin(((location.Longitude*RAD -
                                                                                                 r.RoutePoints.
                                                                                                     FirstOrDefault().
                                                                                                     Longitude*RAD))/2.0)
                                                                                  .Value, 2.0)
                                                                      )
                                                                                        ))
                                                              )).Value * 1000
                                     }).ToList();

                return routes;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error occurred finding routes", ex);
            }
        }

        public Route Details(int routeId)
        {
            return _routeRepository.Find(routeId);
        }

        public Route Details(int routeId, int? accessingAccountId, out bool isFavourited, out bool isOwner)
        {
            Route route = _routeRepository.Find(routeId);
            isFavourited = false;
            isOwner = false;
            if (accessingAccountId != null)
            {
                isFavourited = _favouriteRepository.All.Where(f => f.RouteID == route.RouteID && f.AccountID == accessingAccountId.Value).Any();
                isOwner = route.AccountID == accessingAccountId.Value;
            }
            return route;
        }

        public List<RoutePoint> RoutePoints(int routeId, string sortIndex, string sortOrder, int page, int rows, out int totalPages, out int totalRecords)
        {
            _sortSecurity.ValidateSortOrder(sortOrder);
            _sortSecurity.ValidateSortIndex(sortIndex, new string[] { "TimeRecorded", "Elevation", "Latitude", "Longitude" });
            IQueryable<RoutePoint> query = _routePointRepository.All.Where(rp => rp.RouteID == routeId).OrderBy(sortIndex + " " + sortOrder);

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            totalRecords = query.Count();
            totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            return query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public Route Update(Route route, int updatingAccountId)
        {
            Route updatedRoute = _routeRepository.Find(route.RouteID);
            if (updatedRoute.AccountID != updatingAccountId)
            {
                throw new SecurityException("Attempt to update route not owned by user.");
            }
            updatedRoute.Name = route.Name;
            _routeRepository.InsertOrUpdate(updatedRoute, (r) => r.RouteID);
            _unitOfWork.Save();
            return updatedRoute;
        }
    }
}