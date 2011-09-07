using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.ViewModels.Mappers
{
    public class RouteToRouteResultMapper : IRouteToRouteResultMapper
    {
        public RouteSummary FromRoute(Route route, Account account)
        {
            return new RouteSummary
            {
                AverageRating = route.AverageRating,
                CreatedBy = route.Account.Username,
                DateCreated = route.DateCreated,
                Name = route.Name,
                Reviewed = account != null && route.RouteReviews.Where(rv => rv.RouteID == route.RouteID && rv.AccountID == account.AccountID).Any(),
                RouteID = route.RouteID
            };
        }
    }
}