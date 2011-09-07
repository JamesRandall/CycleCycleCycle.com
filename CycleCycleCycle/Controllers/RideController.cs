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
    public class RideController : BaseController
    {
        private readonly IRideService _rideService;

        public RideController(IAccountService accountService, IRideService rideService)
            : base(accountService)
        {
            _rideService = rideService;
        }

        //
        // POST: /Ride/
        [HttpPost]
        [Authorize]
        public JsonResult Create(int routeId, DateTime dateRidden, int? hours, int? minutes, int? seconds)
        {
            bool saved = false;
            
            try
            {
                saved = _rideService.Create(Account.AccountID, routeId, dateRidden, hours, minutes, seconds);
            }
            catch (Exception)
            {
                saved = false;
            }

            return Json(saved);
        }

    }
}
