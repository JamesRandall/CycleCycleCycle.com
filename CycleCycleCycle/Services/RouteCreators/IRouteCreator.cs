using System.Xml.Linq;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.Services.RouteCreators
{
    public interface IRouteCreator
    {
        Route CreateFromXDocument(XDocument routeDocument);
    }
}
