using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services;
using CycleCycleCycle.ViewModels;
using CycleCycleCycle.ViewModels.Mappers;

namespace CycleCycleCycle.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IRouteToRouteResultMapper _routeToRouteResultMapper;
        //
        // GET: /Profile/

        public ProfileController(IAccountService accountService, IRouteToRouteResultMapper routeToRouteResultMapper) : base(accountService)
        {
            _routeToRouteResultMapper = routeToRouteResultMapper;
        }

        public ActionResult Index()
        {
            Profile profile = new Profile
                                  {
                                      Favourites =
                                          Account.Favourites.Select(f => f.Route).OrderByDescending(r => r.AverageRating)
                                          .ThenBy(r => r.DateCreated).ToList().Select(r => _routeToRouteResultMapper.FromRoute(r, Account)),
                                      Reviews = Account.RouteReviews.OrderByDescending(r => r.DateCreated),
                                      Uploads = Account.Routes.OrderByDescending(r => r.DateCreated).ToList().Select(f => _routeToRouteResultMapper.FromRoute(f, Account)),
                                      Rides = Account.Rides.OrderByDescending(r => r.Route.DateCreated).ToList(),
                                      TotalDistanceCycled = Account.Rides.Sum(r => r.Route.Distance),
                                      TotalAscent = Account.Rides.Sum(r => r.Route.TotalAscent),
                                      TotalDescent = Account.Rides.Sum(r => r.Route.TotalDescent)
                                  };
            return View(profile);
        }

    }
}
