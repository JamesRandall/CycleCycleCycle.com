using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.Services
{
    public interface IRouteReviewService
    {
        bool IsRouteReviewedByUser(int routeId, int accountId);
        RouteReview Insert(RouteReview routeReview);
    }
}
