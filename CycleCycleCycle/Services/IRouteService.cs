using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using CycleCycleCycle.Models;
using CycleCycleCycle.ViewModels;

namespace CycleCycleCycle.Services
{
    public interface IRouteService
    {
        Route CreateFromXDocument(XDocument routeDocument, int accountId);
        Bitmap HeightMapImage(int routeId, int width, int height);
        List<RouteSummary> FindRoute(string searchString, double distanceFromStart, out string searchedLocation);
        TopRoutes TopRoutes(Account account);
        Route Details(int routeId);
        Route Details(int routeId, int? accessingAccountId, out bool isFavourited, out bool isOwner);
        List<RoutePoint> RoutePoints(int routeId, string sidx, string sord, int page, int rows, out int totalPages, out int totalRecords);
        Route Update(Route route, int updatingAccountId);
        Stream Download(int id);
    }
}