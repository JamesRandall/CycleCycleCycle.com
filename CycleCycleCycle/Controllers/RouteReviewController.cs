using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services;

namespace CycleCycleCycle.Controllers
{
    public class RouteReviewController : BaseController
    {
        private readonly IRouteService _routeService;
        private readonly IRouteReviewService _routeReviewService;

        public RouteReviewController(IAccountService accountService, IRouteService routeService, IRouteReviewService routeReviewService) : base(accountService)
        {
            _routeService = routeService;
            _routeReviewService = routeReviewService;
        }

        //
        // GET: /RouteReview/Create/5
        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.RouteName = _routeService.Details(id).Name;
            RouteReview review = new RouteReview();
            review.RouteID = id;
            review.Rating = 3;
            return View(review);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(RouteReview routeReview)
        {            
            if (ModelState.IsValid)
            {
                if (_routeReviewService.IsRouteReviewedByUser(routeReview.RouteID, Account.AccountID))
                {
                    ModelState.AddModelError("", "You have already written a review for this route.");
                    return View(routeReview);
                }

                routeReview.AccountID = Account.AccountID;
                routeReview = _routeReviewService.Insert(routeReview);

                return RedirectToAction("Details", "Route", new { id = routeReview.RouteID });
            }
            else
            {
                ViewBag.RouteName = _routeService.Details(routeReview.RouteID).Name;
                return View(routeReview);
            }
        }

    }
}
