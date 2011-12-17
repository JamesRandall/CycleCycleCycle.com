using System.Collections.Generic;
using CycleCycleCycle.ViewModels;

namespace CycleCycleCycle.Services.Utilities
{
    public interface IGeographicRouteLocator
    {
        List<RouteSummary> FindRoute(string searchString, double distanceFromStart, out string searchedLocation);
    }
}