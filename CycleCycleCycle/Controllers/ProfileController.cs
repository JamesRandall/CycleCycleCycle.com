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

        
        //
        // GET: /id
        public ActionResult Index(string id)
        {
            Account account;
            bool isOwnProfile;

            if (!String.IsNullOrEmpty(id))
            {
                account = AccountService.Find(id);
                isOwnProfile = Account == account;
            }
            else
            {
                account = Account;
                isOwnProfile = true;
            }
            
            Profile profile = new Profile
            {
                Favourites =
                    account.Favourites.Select(f => f.Route).OrderByDescending(r => r.AverageRating)
                    .ThenBy(r => r.DateCreated).ToList().Select(r => _routeToRouteResultMapper.FromRoute(r, account)),
                Reviews = account.RouteReviews.OrderByDescending(r => r.DateCreated),
                Uploads = account.Routes.OrderByDescending(r => r.DateCreated).ToList().Select(f => _routeToRouteResultMapper.FromRoute(f, account)),
                Rides = account.Rides.OrderByDescending(r => r.Route.DateCreated).ToList(),
                TotalDistanceCycled = account.Rides.Sum(r => r.Route.Distance),
                TotalAscent = account.Rides.Sum(r => r.Route.TotalAscent),
                TotalDescent = account.Rides.Sum(r => r.Route.TotalDescent),
                IsOwnProfile = isOwnProfile,
                Username = account.Username
            };
            return View(profile);
        }
    }
}
