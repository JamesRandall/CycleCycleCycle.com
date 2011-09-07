using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.ViewModels.Mappers
{
    public interface IRouteToRouteResultMapper
    {
        RouteSummary FromRoute(Route route, Account account);
    }
}
